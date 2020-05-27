
[iTC_CC_ATP-SwRS-0751]
NoDangerForMemorizedLocationOverSpeed，在使用记忆定位而还未读到确认信标时，ATP监控列车速度是否超过项目限制值。
```
	def NoDangerForMemorizedLocationOverSpeed(k):
	    return (not MemLocationNotConfirmed(k)
	            or TrainMaxSpeed < ATPsetting.MemLocLimitSpeed)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1272], [iTC_CC_ATP_SwHA-0266]
[End]
[iTC_CC_ATP-SwRS-0719]
EBforMemorizedLocationOverSpeed，在使用记忆定位而还未读到确认信标时，ATP应确保列车速度不超过项目限制值。
```
	def EBforMemorizedLocationOverSpeed(k):
	    return (not NoDangerforMemorizedLocationOverSpeed(k)
	            and not MotionProtectionInhibition(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1272], [iTC_CC_ATP_SwHA-0266]
[End]
