﻿
当列车车速大于项目配置数据时，ATP应当要求列车锁闭车厢两侧的车门；只有当车速低于项目配置数据的值，且该侧车门方向允许逃生（由线路上的禁止逃生区限制）或者项目没有配置驾驶室的端门时，ATP才允许列车解除该侧车门的锁闭请求。
When the train speed is higher than project configuration data, ATP shall request the train to close and lock on both sides of the door. Only when the speed is lower than the value of the project configuration data, and this side permits for the escape (is determined by restricted escape area in the lines), or there is no end door configured in the cab, ATP shall allow to release the door locking request. 
[iTC_CC_ATP-SwRS-0352]
HoldDoorsClosed_A，A侧车门锁闭.
The conditions ATP determining the HoldDoorsClosed_A show as following ARDL:
```
	if (Initialization
	     or (ValidTrainKinematic(k) != True))
	    HoldDoorsClosed_A = False
	elif (TrainMaxSpeed(k) > ATPsetting.DoorTrainLockingSpeed)
	    HoldDoorsClosed_A = True
	elif (TrainMaxSpeed(k) <= ATPsetting.DoorTrainUnlockingSpeed)
	    HoldDoorsClosed_A(k)
	      = ((EvacuationNotPossible_A(k) and (not EvacuationNotPossible_B(k)))
	         or (EvacuationNotPossible_A(k) and EvacuationNotPossible_B(k)
	              and ATPsetting.EvacuationTrainEnd))
	else:
	    HoldDoorsClosed_A = HoldDoorsClosed_A(k-1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0240], [iTC_CC-SyAD-0244], [iTC_CC-SyAD-0245], [iTC_CC_ATP_SwHA-0149]
[End]
[iTC_CC_ATP-SwRS-0353]
HoldDoorsClosed_B，B侧车门锁闭
The conditions ATP determining the HoldDoorsClosed_B show as following ARDL :
```
	if (Initialization
	     or (ValidTrainKinematic(k) != True))
	    HoldDoorsClosed_B = False
	elif (TrainMaxSpeed(k) > ATPsetting.DoorTrainLockingSpeed)
	    HoldDoorsClosed_B = True
	elif (TrainMaxSpeed(k) <= ATPsetting.DoorTrainUnlockingSpeed)
	    HoldDoorsClosed_B(k)
	      = ((EvacuationNotPossible_B(k) and (not EvacuationNotPossible_A(k)))
	         or (EvacuationNotPossible_A(k) and EvacuationNotPossible_B(k)
	              and ATPsetting.EvacuationTrainEnd))
	else:
	    HoldDoorsClosed_B = HoldDoorsClosed_B(k-1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0240], [iTC_CC-SyAD-0244], [iTC_CC-SyAD-0245], [iTC_CC_ATP_SwHA-0149]
[End]
