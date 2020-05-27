﻿
在BM模式下，在每周期应明确使用来自CBI无线的变量还是BM信标的变量。使用这两种变量均是安全的，需要从可用性角度，判断哪种更加新，则使用该变量。原则如下：
初始化时，或项目未配置CBI无线，则使用BM信标变量；
否则，如果列车失位或LocationPathKnown未知，则使用BM信标变量；
否则，如果之前未收到BM信标，或BM信标变量不可用，则使用CBI无线变量；
否则，如果CBI无线变量在有效期内；且比之前收到的BM信标加传输延迟更新；且该无线变量是在BMCP点上游的接收窗之后收到的，则使用CBI无线变量。
否则，使用BM信标变量。
[iTC_CC_ATP-SwRS-0627]
CBIvariantMoreAvailableThanBeacon，通过比较最后一次收到的BM信标的有效期，和对应变量所在该联锁区的无线变量，判断对于该变量，是使用来自CI无线的变量而非来自信标的变量。
ATP shall use the more recent message from beacons and CBI radio.
```
	def CBIvariantMoreAvailableThanBeacon(CbiId, k):
	    if (Initialization
	        or not TrainLocatedOnKnownPath(k-1)
	        or not ATPsetting.BlockModeThroughRadio(k)):
	        return False
	    else:
	        return (UsedBMbeaconId(k) is None
	                or (CBIvariantAge(CbiId, k) <= ATPsetting.VariantsBMfullValidityTime
	                     and (CBIvariantReportLastAge(CbiId, k)
	                          <= BMbeaconReadAge(k) + ATPsetting.VariantsBMradioPriorityDelay)
	                     and (CBIvariantReportLastAge(CbiId, k) <= CBIminProductionAge(CbiId, k))))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0108], [iTC_CC-SyAD-0295], [iTC_CC-SyAD-1163], [iTC_CC_ATP_SwHA-0275]
[End]
NOTES：
判断条件CBIvariantReportLastAge <= CBIminProductionAge，表明当前使用的无线消息是列车进入BMCP点上游 “Reception Windows”之后收到的新的消息（或者列车还未经过BMCP点），因此可以使用。
若不满足这个条件，表明列车经过了BMCP点，但未在“Reception Windows”内收到新的无线消息，因此不能相信；
此时应使用来自信标的消息（该信标应当布置在Reception Windows之中，且由配置Vital zone保证不能丢失）。
可参考需求CBIminProductionAgeSinceSSAcrossing和CBIminProductionAge。
[iTC_CC_ATP-SwRS-0628]
BMvariantValue，统一来自BM信标和CBI无线的BM变量
```
	def BMvariantValue(Variant, k):
	    if (CBIvariantMoreAvailableThanBeacon(Variant.Cbi.Id, k)):
	        return BMcbivariantValue(Variant.Cbi.Id, Variant.Cbi.Index, k)
	    else:
	        return BMbeaconVariantValue(Variant.LineSec.Id, Variant.LineSec.Index, k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0295], [iTC_CC-SyAD-0296], [iTC_CC-SyAD-1179] , [iTC_CC_ATP_SwHA-0275]
[End]
[iTC_CC_ATP-SwRS-0629]
BMvariantRemainingTime，BM变量的剩余有效期
```
	def BMvariantRemainingTime(cbi, k):
	    if (not BMvariantValidWhileTemporallyValid(k)):
	        return 0
	    elif (CBIvariantMoreAvailableThanBeacon(cbi, k)):
	        return max(0, ATPsetting.VariantsBMfullValidityTime - CBIvariantAge(cbi, k))
	    else:
	        return max(0, ATPsetting.VariantsBMfullValidityTime - BMbeaconReadAge(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0290], [iTC_CC-SyAD-1189], [iTC_CC_ATP_SwHA-0235]
[End]