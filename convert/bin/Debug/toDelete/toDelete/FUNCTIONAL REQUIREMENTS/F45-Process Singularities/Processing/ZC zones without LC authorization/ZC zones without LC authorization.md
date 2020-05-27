
CBTC模式下非LC授权的ZC边界：
CBTC模式下，如果从列车车尾最小定位到EB实际施加位置的范围内存在非LC授权的ZC区域，ATP需将其作为安全速度限制区域处理，其限速值为0；
CBTC模式下，如果非授权的ZC区域边界在EBA点下游，则ATP将其作为安全限速点，限制点为0。
[iTC_CC_ATP-SwRS-0711]
ZoneVSLnotExceedZC，非授权ZC作为区域型限速的情形
```
	def ZoneVSLnotExceedZC(k):
	    for block in (TrackMap.AllSingsBtwTwoLocs(SGL_NEW_BLOCK, TrainRearLocation(k).Min,
	                              TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max, X2EbApplied(k)))):
	        if (not BlockModeUsed(k)
	            and not VersionAuthorizedByLC(TrackMap.ZCId(block.Id), k)):
	            return False
	        else:
	            continue
	    return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0294], [iTC_CC-SyAD-0300], [iTC_CC_ATP_SwHA-0258]
[End]
[iTC_CC_ATP-SwRS-0712]
PointVSLnotExceedZC，非授权ZC边界作为点型限速的情形
```
	def PointVSLnotExceedZC(k):
	    for NewBlock in (TrackMap.AllSingsInZone(SGL_NEW_BLOCK, 
	                                TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max, X2EbApplied(k)),
	                                ATPsetting.EOAmaxDistance)):
	        if (not BlockModeUsed(k)
	            and not VersionAuthorizedByLC(TrackMap.ZCId(NewBlock.Id), k)
	            and TrainEnergy(k) >= (Energy.AccumulationPotentialEnergy
	                                       (TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max,
	                                                                           X2EbApplied(k)),
	                                       NewBlock.Location))):
	            return False
	        else:
	            continue
	    return True
```
其中NewBlock.Location表示该block的起始位置。
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0294], [iTC_CC-SyAD-0300], [iTC_CC-SyAD-0311], [iTC_CC-SyAD-0314], [iTC_CC_ATP_SwHA-0258]
[End]
