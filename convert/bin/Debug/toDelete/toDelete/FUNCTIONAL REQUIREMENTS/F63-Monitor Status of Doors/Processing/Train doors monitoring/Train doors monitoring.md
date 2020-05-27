
若列车停在车站，当车门开启时，ATP应当保持输出停车制动 
When the train stopped and opened the door in the station, ATP shall keep outputting parking brake.
[iTC_CC_ATP-SwRS-0070]
AllTrainDoorsClosedAndLocked，两端车头有任意一端采到TDCL，即认为两侧车门关闭并锁闭。
The AllTrainDoorsClosedAndLocked stands for the condition that either side of both train ends collect TDCL, i.e. both side of door is closed and locked.
```
	def AllTrainDoorsClosedAndLocked(k):
	    return Offline.GetAllTrainDoorsClosedAndLocked(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0239], [iTC_CC-SyAD-0241], [iTC_CC-SyAD-1003], [iTC_CC_ATP_SwHA-0268]
[End]
[iTC_CC_ATP-SwRS-0337]
InhibitControlTrainDoorsStatus，不监控车门状态.
ATP shall not monitor the status of train doors when InhibitControlTrainDoorsStatus is selected.
```
	def InhibitControlTrainDoorsStatus(k):
	    return Offline.GetInhibitControlTrainDoorsStatus(k) 
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1220], [iTC_CC-SyAD-1303]
[End]
[iTC_CC_ATP-SwRS-0804]
NoDangerForTrainDoorsNotClosedAndLocked，当列车停车，且与PSD区或VPEZ有交集时，且TDCL丢失，则该值为假；否则，该值为真。
```
	def NoDangerForTrainDoorsNotClosedAndLocked(k):
	    return not (TrainFilteredStopped(k)
	                 and not AllTrainDoorsClosedAndLocked(k)
	                 and (AlignPSDzone_A(k) or AlignPSDzone_B(k)
	                      or TrainInterVPEZ_A(k) or TrainInterVPEZ_B(k)))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0230], [iTC_CC-SyAD-0231]
[End]
[iTC_CC_ATP-SwRS-0338]
PBforTrainDoorsNotClosedAndLocked，列车停车，且车身与PSD区或VPEZ区域有交集时，车门未关时保持PB输出。
If the train is aligning in a PSD or intersecting with a vital passage exchange zone, and the RMF or RMR does not selected, ATP shall keep triggering parking brake when the train doors does not closed and locked.
```
	def PBforTrainDoorsNotClosedAndLocked(k):
	     return (not NoDangerForTrainDoorsNotClosedAndLocked(k)
	              and not InhibitControlTrainDoorsStatus(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0230], [iTC_CC-SyAD-0231], [iTC_CC-SyAD-1220], [iTC_CC-SyAD-1226], [iTC_CC_ATP_SwHA-0141]
[End]
[iTC_CC_ATP-SwRS-0339]
EBforPBnotAppliedDueToTrainDoors，由于车门开而输出ZVRD，但未检测到ZVBA, 则ATP应当输出EB.
If ATP has triggered parking brake for train doors opening, but it does not applied by the rolling stock, ATP shall trigger the emergency brake.
```
	EBforPBnotAppliedDueToTrainDoors(k)
	 = ((PBforTrainDoorsNotClosedAndLocked(k) == True)
	   and (TrainParkingBrakeApplied(k) != True))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0233], [iTC_CC_ATP_SwHA-0142]
[End]
[iTC_CC_ATP-SwRS-0340]
NoDangerForDepartureWithoutTDCL，判断是否未处于上周期停车而本周期开始动车，且车门未关的条件。
ATP shall determine whether the train is departure without TDCL.
```
	def NoDangerForDepartureWithoutTDCL(k):
	    return (AllTrainDoorsClosedAndLocked(k)
	            or TrainFilteredStopped(k)
	            or not TrainFilteredStopped(k-1))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC_ATP_SwHA-0143]
[End]
[iTC_CC_ATP-SwRS-0749]
EBforDepartureWithoutTDCL，若ATP监控发车时丢失TDCL的情况，则输出EB。
If ATP needs to monitor the status of train doors, ATP shall trigger EB if train determine without TDCL:
```
	def EBforDepartureWithoutTDCL(k):
	    return (not NoDangerForDepartureWithoutTDCL(k)
	            and not InhibitControlTrainDoorsStatus(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1226], [iTC_CC-SyAD-1227], [iTC_CC_ATP_SwHA-0143]
[End]
NOTES：
对于非授权开门状态下车门打开（MWTDCL）监控与EvacuationWhileLeavingStation功能（EWLS）的区别及其配置说明：
已有的EWLS功能SwRS-0365，在离站距离内（车门授权DE由真变假后开始记录），车动且乘客手柄丢失，会EB；
而MWTDCL，与之有两个区别：
车辆输入的乘客紧急手柄EmergencyHandleNotPulledSide和车门状态AllTrainDoorsClosedAndLocked是否合一；
项目能否实现Door opening authorization开门授权功能（例如不采集ZVBA输入，则不会满足开门授权条件）。
因此，可根据项目要求，选择配置MWTDCL和EWLS这两个功能，如下表所示。
Table 513 Configuration for monitoring unexpected train door open

|Project configuration|With  REF _Ref385344693 \h Door opening authorization|Without  REF _Ref385344693 \h Door opening authorization|
|-----|
|Source of EHNPS and TDCL from RS are same|Current metro projects:-- Can modify evacuation station area length for EWLS to fulfill the project-- MWTDCL is unnecessary|Tramcar project for Ethiopia:-- EWLS is disabled-- Set MWTDCL according to project|
|Source of EHNPS and TDCL from RS are independent|Maybe future metro projects:-- set EWLS to monitor EHNPS-- set MWTDCL to monitor TDCL||

[iTC_CC_ATP-SwRS-0799]
InhibitProtectionMovingWithoutTDCL，禁止监控非开门授权情况下车门打开的情形。
ATP shall not monitor the status of train doors open without door opening enable if InhibitProtectionMovingWithoutTDCL is selected.
```
	def InhibitProtectionMovingWithoutTDCL(k):
	    return Offline.GetInhibitProtectionMovingWithoutTDCL(k) 
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1441]
[End]
[iTC_CC_ATP-SwRS-0800]
NoDangerForMovingWithoutTDCL，监控非授权开门状态下车门打开
```
	def NoDangerForMovingWithoutTDCL(k):
	    return (AllTrainDoorsClosedAndLocked(k)
	            or TrainFilteredStopped(k)
	            or EnableDoorOpening_A(k)
	            or EnableDoorOpening_B(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1441]
[End]
[iTC_CC_ATP-SwRS-0801]
EBforMovingWithoutTDCL，禁止监控非开门授权情况下车门打开的情形。
```
	def EBforMovingWithoutTDCL(k):
	    return (not NoDangerForMovingWithoutTDCL(k)
	            and not InhibitProtectionMovingWithoutTDCL(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1441]
[End]
