
当列车在站内发车，或者未完全出站时，如果乘客拉下了两侧车门的紧急逃生手柄，则ATP应当输出EB，使得列车停止；但如果列车已在区间运行，乘客拉下紧急逃生手柄，则ATP不应当输出EB。
When the train is just leaving from the platform or stops out of the station, if the passenger pulls down the emergency handle, ATP shall trigger EB to stop the train. If the train is running on the interval region, when the passenger pulls down the handle, the ATP shall not output EB.
[iTC_CC_ATP-SwRS-0354]
TrainDockedInStation，根据开门授权条件判断是否车停在站内。
ATP shall determine whether the train has docked in the station correctly according to conditions of train stopping and doors opening enable.
```
	def TrainDockedInStation(k):
	    return (TrainFilteredStopped(k)
	             and (EnableDoorOpening_A(k)
	                  or EnableDoorOpening_B(k)))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0229]
[End]
[iTC_CC_ATP-SwRS-0355]
TrainLeavingStation，判断是否在离站过程中。
从TrainDockedInStation由True变为False开始，如果列车测速有效，累加MaximumTrainMotion距离：
如果其绝对值在[0, ATPsetting.EvacuationStationAreaLength]范围内，则设置TrainLeavingStation为True；否则为False。
即如果列车出站后又倒车回到上述范围内，也应认为是TrainLeavingStation。
如果列车运动学无效，则设置该值为False并清除累加距离。
The train is said to be leaving the station:
if since last time train has been detected docked in station (TrainDockedInStation), the cumulated of the absolute value of MaximumTrainMotion is in the range [0, ATPsetting.EvacuationStationAreaLength] and no train kinematic invalidation occurs. 
or else, if the train kinematics is invalid, ATP shall set TrainLeavingStation as False and clear the cumulated distance.
```
	def TrainLeavingStation(k):
	    if (Initialization
	        or not ValidTrainKinematic(k)):
	        TrainHasDockedInStation = False
	        LeavingStationDistance = 0
	        return False
	    elif (TrainDockedInStation(k)):
	        TrainHasDockedInStation = True
	        LeavingStationDistance = 0
	        return False
	    elif (not TrainHasDockedInStation(k)):
	        LeavingStationDistance = 0
	        return False
	    else:
	        LeavingStationDistance = LeavingStationDistance(k-1) + MaximumTrainMotion(k)
	        return (abs(LeavingStationDistance) <= ATPsetting.EvacuationStationAreaLength)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0271], [iTC_CC-SyAD-1231]
[End]
[iTC_CC_ATP-SwRS-0071]
EmergencyHandleNotPulledSide侧向的紧急手柄未落下。其状态来自于项目可配置的列车输入采集。
EmergencyHandleNotPulledSid shows that the emergency handles is not pulled down.
```
	def EmergencyHandleNotPulledSide(k):
	    return Offline.GetEmergencyHandleNotPulledSide(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0269], [iTC_CC-SyAD-1003], [iTC_CC_ATP_SwHA-0207]
[End]
[iTC_CC_ATP-SwRS-0356]
EvacuationWhileLeavingStation，未完全出站时丢失车门状态则EB.
If the train is just leaving the station and the side doors emergency handles are pulled, ATP shall require EvacuationWhileLeavingStation.
```
	EvacuationWhileLeavingStation(k)
	 = ((EmergencyHandleNotPulledSide(k) != True)
	    and (TrainLeavingStation(k) == True)
	    and (TrainFilteredStopped(k) != True))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0271], [iTC_CC-SyAD-1231], [iTC_CC_ATP_SwHA-0150]
[End]
[iTC_CC_ATP-SwRS-0357]
EvacuationWithTrainStopped，非开门区，停车且乘客紧急手柄拉下
If the train does not stop on the doors opening enable area and the side doors emergency handles pulled, ATP shall require EvacuationWithTrainStopped.
```
	EvacuationWithTrainStopped(k)
	 = ((EmergencyHandleNotPulledSide(k) != True)
	    and (TrainFilteredStopped(k) == True)
	    and not EnableDoorOpening_A(k)
	    and not EnableDoorOpening_B(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0274], [iTC_CC_ATP_SwHA-0151]
[End]
[iTC_CC_ATP-SwRS-0726]
InhibitProtectionEvacuationInDistance，在离站时禁止监控逃生手柄状态
```
	def InhibitProtectionEvacuationInDistance(k):
	    return Offline.GetInhibitProtectionEvacuationInDistance(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1231], [iTC_CC-SyAD-1302]
[End]
[iTC_CC_ATP-SwRS-0727]
InhibitProtectionEvacuationWithStop，在站间停车时禁止监控逃生手柄状态。
```
	def InhibitProtectionEvacuationWithStop(k):
	    return Offline.GetInhibitProtectionEvacuationWithStop(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1232], [iTC_CC-SyAD-1301]
[End]
[iTC_CC_ATP-SwRS-0358]
EBforEvacuationWhileTrainLeavingStation，出站时的逃生请求EB.
If the train leaving station evacuation has been required, ATP shall trigger the emergency brake.
```
	def EBforEvacuationWhileTrainLeavingStation(k):
	    return (EvacuationWhileLeavingStation(k)
	             and not InhibitProtectionEvacuationInDistance(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0271], [iTC_CC-SyAD-1231], [iTC_CC_ATP_SwHA-0150]
[End]
[iTC_CC_ATP-SwRS-0748]
EBforEvacuationWithTrainStopped，站间停车时的逃生请求EB.
If the train stopped evacuation has been required, ATP shall trigger the emergency brake.
```
	def EBforEvacuationWithTrainStopped(k):
	    return (EvacuationWithTrainStopped(k)
	              and not InhibitProtectionEvacuationWithStop(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0274], [iTC_CC-SyAD-1232], [iTC_CC_ATP_SwHA-0151]
[End]
