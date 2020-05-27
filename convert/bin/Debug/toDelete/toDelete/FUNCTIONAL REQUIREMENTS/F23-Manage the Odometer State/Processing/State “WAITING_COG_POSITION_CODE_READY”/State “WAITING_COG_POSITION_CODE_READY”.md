
[iTC_CC_ATP-SwRS-0179]
InitializationTimer，在WAITING_COG_POSITION_CODE_READY状态下累加初始化时间.
ATP shall accumulate the time for waiting cog position ready state.
```
	if (OdometerState(k-1) == WAITING_COG_POSITION_CODE_READY
	     and OdometerState(k) == WAITING_COG_POSITION_CODE_READY)
	    InitializationTimer =  InitializationTimer(k-1) + 1 
	elif (OdometerState(k-1) != WAITING_COG_POSITION_CODE_READY
	     and OdometerState(k) == WAITING_COG_POSITION_CODE_READY)
	    InitializationTimer = 1
	else:
	    InitializationTimer = 0
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1160]
[End]
[iTC_CC_ATP-SwRS-0180]
由WAITING_COG_POSITION_CODE_READY转回NOT_INITIALIZED状态的条件：
At cycle k, ATP shall consider that OdometerState changes from WAITING_COG_POSITION_CODE_READY to NOT_INITIALIZED if:
wheel is detected stopped (WheelFilteredStopped),
and cog position remains unknown (not OdometerCogPositionReady),
and there is no sensors test inconsistency,
and time elapsed since last time OdometerState was NOT_INITIALIZED (InitializationTimer) is strictly less than ATPsetting.OdoInitTimeout
```
	if (OdometerState(k-1) = WAITING_COG_POSITION_CODE_READY)
	    and WheelFilteredStopped(k)
	    and not OdometerCogPositionReady(k)
	    and not UnconsistentSensorTest(k)
	    and (InitializationTimer(k) < ATPsetting.OdoInitTimeout)
	   OdometerState =  NOT_INITIALIZED
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0149], [iTC_CC-SyAD-1160]
[End]
[iTC_CC_ATP-SwRS-0181]
由WAITING_COG_POSITION_CODE_READY转入INITIALIZED状态的条件：
At cycle k, ATP shall consider that OdometerState changes from WAITING_COG_POSITION_CODE_READY to INITIALIZED
If:
Cog position is safely known which means that wheel angular position is well-known;
and there is no sensors test inconsistency;
and time elapsed since last time OdometerState was NOT_INITIALIZED (InitializationTimer) is strictly less than ATPsetting.OdoInitTimeout.
```
	if (OdometerState(k-1) == WAITING_COG_POSITION_CODE_READY
	    and OdometerCogPositionReady(k)
	    and not UnconsistentSensorTest(k)
	    and (InitializationTimer(k)< ATPsetting.OdoInitTimeout))
	    OdometerState = INITIALIZED
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0138], [iTC_CC-SyAD-1160], [iTC_CC_ATP_SwHA-0061]
[End]
[iTC_CC_ATP-SwRS-0182]
由WAITING_COG_POSITION_CODE_READY转入INVALID的条件：
At cycle k, ATP shall consider that OdometerState changes from WAITING_COG_POSITION_CODE_READY to INVALID if:
sensors test inconsistency is detected,
or time elapsed since last time OdometerState was NOT_INITIALIZED (InitializationTimer) is more than or equal to the ATPsetting.OdoInitTimeout
```
	if (OdometerState(k-1) ==  WAITING_COG_POSITION_CODE_READY)
	   and ((InitializationTimer(k)>= ATPsetting.OdoInitTimeout)
	          or UnconsistentSensorTest(k))
	    OdometerState = INVALID
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0149], [iTC_CC-SyAD-1160]
[End]
[iTC_CC_ATP-SwRS-0183]
在里程计初始化阶段，ATP需根据当前车头激活方向和上周期位移的结果，对本周期位移进行过估处理。
When odometer is initializing, wheel movement shall be over and under estimated considering maximum acceleration per cycle according with the train front:
```
	if (OdometerState(k) ==  WAITING_COG_POSITION_CODE_READY)
	    if ((TrainFrontEnd(k-1) == END_2) or (NoUndetectableDanger_2(k-1) == True))
	         WheelMinimumMovement(k) = WheelMinimumMovement(k-1) + ATPsetting.MaxMotionPerCycle
	         WheelMaximumMovement(k) = WheelMaximumMovement(k-1) - ATPsetting.MaxMotionPerCycle
	    else:
	         WheelMinimumMovement(k) = WheelMinimumMovement(k-1) — ATPsetting.MaxMotionPerCycle
	         WheelMaximumMovement(k) = WheelMaximumMovement(k-1) + ATPsetting.MaxMotionPerCycle
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0132], [iTC_CC_ATP_SwHA-0062]
[End]
