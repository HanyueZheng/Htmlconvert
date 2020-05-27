
如果ATP检测到司机选择了限制人工（RM）模式，则根据项目配置的RM模式限速值监控列车是否超速。
If the RM forward or reverse modes selected, ATP shall monitor whether the train is overspeed based on the RM limitation.
[iTC_CC_ATP-SwRS-0743]
ConditionForRMlimitSpeed，当前应用哪种RM限速。ATP最多支持项目配置MAX_RM_CONDITION_NB种RM限速。
```
	def ConditionForRMlimitSpeed(i, k):
	    return Offline.GetConditionForRMlimitSpeed(i, k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1265], [iTC_CC-SyAD-1309]
[End]
[iTC_CC_ATP-SwRS-0744]
RMlimitSpeedApplied，根据列车输入，判断当前应当监控的RM限速
```
	def RMlimitSpeedApplied(k):
	    for i in range(0, MAX_RM_CONDITION_NB):
	        if (ConditionForRMlimitSpeed(i, k)):
	            return ATPsetting.MPinhibitionLimitSpeed[i]
	        else:
	            continue
	    return 0
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0278], [iTC_CC-SyAD-1309], [iTC_CC-SyAD-1394]
[End]
NOTES:
在配置数据中，ConditionForRMlimitSpeed必须配置列车的“各种模式”及其相应的“最大”限速。其原因是防止ATP未发现相应模式的限速时，将RMlimitSpeedApplied设置为0，影响可用性。但对于这些限速的监控，仅在MotionProtectionInhibition时实施。
[iTC_CC_ATP-SwRS-0497]
NoDangerForRMoverSpeed，列车速度小于等于RM模式下的限速。
ATP estimates that current train maximum speed not exceeds the RM limit speed.
```
	def NoDangerForRMoverSpeed(k):
	    return (ValidTrainKinematic(k)
	            and TrainMaxSpeed(k) <= RMlimitSpeedApplied(k)) 
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0275], [iTC_CC-SyAD-0278], [iTC_CC-SyAD-0364], [iTC_CC-SyAD-1394], [iTC_CC_ATP_SwHA-0121]
[End]
[iTC_CC_ATP-SwRS-0734]
EBforRMoverSpeed，若在RM模式下，列车速度大于RM模式限速，则将输出EB。
```
	def EBforRMoverSpeed(k):
	    return (not NoDangerforRMoverSpeed(k)
	             and MotionProtectionInhibition(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0275], [iTC_CC-SyAD-0278], [iTC_CC-SyAD-0364], [iTC_CC-SyAD-1394], [iTC_CC_ATP_SwHA-0121]
[End]
