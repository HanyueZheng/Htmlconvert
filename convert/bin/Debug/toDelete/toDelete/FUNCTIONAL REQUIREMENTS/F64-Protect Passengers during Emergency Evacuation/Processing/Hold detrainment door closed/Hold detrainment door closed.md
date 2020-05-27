
对于配有两端驾驶室逃生门的项目，当列车静止或运动学测量失效时，如果端门的紧急逃生手柄被拉下，则ATP应当触发EB，解锁端门，供乘客逃生。
For the project equipped with the detrained door in driving cab, when the train stopped or dynamic measurement invalid, if emergency handle is pulling down, the ATP shall trigger EB, unlock the door for passengers getting out.
[iTC_CC_ATP-SwRS-0072]
EmergencyHandleNotPulledEnd1，END_1逃生门未开。如果该项目未配置驾驶室的逃生门，则认为该逃生门未开。其状态来自于项目可配置的列车输入采集。
EmergencyHandleNotPulledEnd1 stands for the closed emergency door of END_1. If the train does not allocate with emergency door in the cab, it is certain that the emergency door does not opened. 
```
	def EmergencyHandleNotPulledEnd1(k):
	    return Offline.GetEmergencyHandleNotPulledEnd1(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0246], [iTC_CC-SyAD-1003], [iTC_CC_ATP_SwHA-0207]
[End]
[iTC_CC_ATP-SwRS-0724]
EmergencyHandleNotPulledEnd2，End_2逃生门未开。如果该项目未配置驾驶室的逃生门，则认为该逃生门未开。其状态来自于项目可配置的列车输入采集。
EmergencyHandleNotPulledEnd2 stands for the closed emergency door of End2. If the train does not allocate with emergency door in the cab, it is certain that the emergency door does not opened. 
```
	def EmergencyHandleNotPulledEnd2(k):
	    return Offline.GetEmergencyHandleNotPulledEnd2(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0246], [iTC_CC-SyAD-1003], [iTC_CC_ATP_SwHA-0207]
[End]
[iTC_CC_ATP-SwRS-0349]
HoldDoorsClosedTrainEnd1，未拉END_1端驾驶室的逃生门紧急手柄，或者车在运动时，锁闭END_1端逃生门。
ATP shall keep hold the train END_1 door closed when one of the following conditions fulfilled:
Train kinematics is valid and the train does not stop;
Or the emergency handle of END_1 is not pulled;
```
	HoldDoorsClosedTrainEnd1(k)
	 = ((ValidTrainKinematic(k) and (not TrainFilteredStopped(k)))
	     or EmergencyHandleNotPulledEnd1(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0248], [iTC_CC-SyAD-0249]
[End]
[iTC_CC_ATP-SwRS-0350]
HoldDoorsClosedTrainEnd2，未拉END_2端驾驶室的逃生门紧急手柄，或者车在运动时，锁闭END_2端逃生门。
ATP shall keep hold the train END_2 door closed when one of the following conditions fulfilled:
Train kinematics is valid and the train does not stop;
Or the emergency handle of END_2 is not pulled;
```
	HoldDoorsClosedTrainEnd2(k)
	 = ((ValidTrainKinematic(k) and (not TrainFilteredStopped(k)))
	    or EmergencyHandleNotPulledEnd2(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0248], [iTC_CC-SyAD-0250]
[End]
[iTC_CC_ATP-SwRS-0351]
EBforNotAllTrainEndHoldDoorsClosed，驾驶室逃生门手柄拉下.
If ATP does not hold the train end door, then trigger emergency brake.
```
	EBforNotAllTrainEndHoldDoorsClosed(k)
	 = (not HoldDoorsClosedTrainEnd1(k)
	     or not HoldDoorsClosedTrainEnd2(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0248], [iTC_CC_ATP_SwHA-0148]
[End]
[iTC_CC_ATP-SwRS-0738]
EmergencyDetrainDoorLockingEnd1，要求车辆锁闭End1端驾驶室的紧急逃生门。
```
	def EmergencyDetrainDoorLockingEnd1(k):
	    return (HoldDoorsClosedTrainEnd1(k)
	            or InhibitEmergencyBrake(k-1))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0249]
[End]
[iTC_CC_ATP-SwRS-0739]
EmergencyDetrainDoorLockingEnd2，要求车辆锁闭End2端驾驶室的紧急逃生门。
```
	def EmergencyDetrainDoorLockingEnd2(k):
	    return (HoldDoorsClosedTrainEnd2(k)
	            or InhibitEmergencyBrake(k-1))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0250]
[End]
