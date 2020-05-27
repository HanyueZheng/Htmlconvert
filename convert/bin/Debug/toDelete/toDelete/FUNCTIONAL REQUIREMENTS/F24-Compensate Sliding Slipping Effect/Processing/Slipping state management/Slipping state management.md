
[iTC_CC_ATP-SwRS-0769]
StartSlippingSpeed，记录由COASTING或MOTORING进入SLIPPING状态时的速度。
ATP shall record the speed when the train begins to slip.
```
	def StartSlippingSpeed(k):
	    if (Initialization
	        or OdometerState(k-1) is not INITIALIZED
	        or MotionUnderEstimationState(k) is COASTING
	        or MotionUnderEstimationState(k) is MOTORING):
	        return 0
	    elif ((MotionUnderEstimationState(k-1) is COASTING
	            or MotionUnderEstimationState(k-1) is MOTORING)
	           and MotionUnderEstimationState(k) is SLIPPING):
	        return WheelMinSpeed(k-1)
	    else:
	        return StartSlippingSpeed(k-1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-
0131][End]
[iTC_CC_ATP-SwRS-0770]
TimeInSlipping，记录在SLIPPING状态下持续了多少个周期.
ATP shall record how many cycles staying in SLIPPING state.
```
	def TimeInSlipping(k):
	    if (Initialization
	        or (MotionUnderEstimationState(k-1) is SLIPPING
	            and MotionUnderEstimationState(k) is not SLIPPING)):
	        return 0
	    elif (MotionUnderEstimationState(k) is SLIPPING):
	        return TimeInSlipping(k-1) + 1
	    else:
	        return TimeInSlipping(k-1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-
0148][End]
[iTC_CC_ATP-SwRS-0793]
OdometerAxleMotorized，表示需考虑里程计所安在车轴牵引导致的空转。
If the project that odometer installed on the traction axle of the train, ATP shall consider the slipping effect to impact the underestimation of measured wheel movement.
```
	def OdometerAxleMotorized(k):
	    return not ATPsetting.OdoNotOnMotorizedAxle
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0131], [iTC_CC_ATP_SwHA-0070]
[End]
[iTC_CC_ATP-SwRS-0199]
对于车辆位移的打滑空转补偿状态MotionUnderEstimationState如下：
COASTING, 无打滑发生；
MOTORING，一般牵引状态（仅在需考虑空转补偿的项目）；
SLIPPING，可补偿的空转状态（仅在需考虑空转补偿的项目）；
SKIDDING, 无法靠里程计补偿的打滑或空转状态。
各个状态的转换关系如Figure 59所示。
ATP software shall use the over-estimation model for train movement provided by Figure 59 state-diagram. The maximum and minimum train motion shall overestimate based on different state as follows:
COASTING. There is not sliding effect during on train coasting or motoring, so ATP need not to overestimate train motion.
MOTORING, normal traction state (only consdering odometer installed on motorized axle).
SLIPPING, wheel slipping happen (only consdering odometer installed on motorized axle).
SKIDDING, If train slides or slips excessively, ATP shall consider odometer motion untrustworthy.
 Figure 59 Processing of under estimation state 
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0131], [iTC_CC_ATP_SwHA-0071]
[End]
[iTC_CC_ATP-SwRS-0771]
The MotionUnderEstimationState transfers from “COASTING” to “MOTORING” when: 
```
	if (MotionUnderEstimationState(k-1) is COASTING
	    and OdometerAxleMotorized(k)
	    and FilteredWheelAcceleration(k) <= ATPsetting.SlippingStartAcc
	    and FilteredWheelAcceleration(k) > ATPsetting.TractionStartAcc
	    and OdometerState(k) is INITIALIZED)
	    MotionUnderEstimationState = MOTORING
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0148],
 [iTC_CC-SyAD-0133][End]
[iTC_CC_ATP-SwRS-0772]
The MotionUnderEstimationState transfers from “COASTING” to “SLIPPING” when: 
```
	if (MotionUnderEstimationState(k-1) is COASTING
	    and OdometerAxleMotorized(k)
	    and FilteredWheelAcceleration(k) > ATPsetting.SlippingStartAcc
	    and OdometerState(k) is INITIALIZED)
	    MotionUnderEstimationState = SLIPPING
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0148], [iTC_CC_ATP_SwHA-0073]
[End]
[iTC_CC_ATP-SwRS-0773]
The MotionUnderEstimationState transfers from “MOTORING” to “SLIPPING” when: 
```
	if (MotionUnderEstimationState(k-1) is MOTORING
	    and FilteredWheelAcceleration(k) > ATPsetting.SlippingStartAcc
	    and AverageWheelAcceleration(k) > ATPsetting.MotoringStartAc)
	    and OdometerState(k) is INITIALIZED
	    and OdometerAxleMotorized(k))
	    MotionUnderEstimationState = SLIDING
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-
0148][End]
[iTC_CC_ATP-SwRS-0774]
The MotionUnderEstimationState transfers from MOTORING to COASTING” when: 
```
	if (MotionUnderEstimationState(k-1) is MOTORING
	    and (AverageWheelAcceleration(k) <= ATPsetting.TractionStartAcc
	          or OdometerState(k) is INVALID
	          or not OdometerAxleMotorized(k)))
	    MotionUnderEstimationState = COASTING
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-
0148][End]
[iTC_CC_ATP-SwRS-0775]
The MotionUnderEstimationState transfers from SLIPPING to COASTING when:
```
	if (MotionUnderEstimationState(k-1) is SLIPPING
	     and (OdometerState(k) is INVALID
	           or not OdometerAxleMotorized(k)))
	    MotionUnderEstimationState = COASTING
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0148], [iTC_CC_ATP_SwHA-0078]
[End]
[iTC_CC_ATP-SwRS-0776]
SlippingEnded，判断是否结束空转状态的条件之一。
```
	def SlippingEnded(k):
	  if (MotionUnderEstimationState(k-1) is not SLIPPING):
	      slipping_ended_counter = 0
	      return False
	  elif (FilteredWheelAcceleration(k) < ATPsetting.SlippingStopAcc
	        and FilteredWheelAcceleration(k) > ATPsetting.SlidingStopAcc):
	      slipping_ended_counter(k) = slipping_ended_counter(k-1) + 1
	      return (slipping_ended_counter >= ATPsetting.SlippingGripRecoveryTime)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-
0148][End]
[iTC_CC_ATP-SwRS-0777]
The MotionUnderEstimationState transfers from SLIPPING to MOTORING when:
```
	if (MotionUnderEstimationState(k-1) is SLIPPING
	     and OdometerState(k) is INITIALIZED
	     and OdometerAxleMotorized(k)
	     and TimeInSlipping(k-1) <= ATPsetting.SlippingTimeout
	     and ((StartSlippingSpeed(k-1) + TimeInSlipping(k-1)* ATPsetting.SlippingStopAcc)
	           > WheelMinSpeed(k))
	     and SlippingEnded(k))
	    MotionUnderEstimationState = MOTORING
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-
0148][End]
[iTC_CC_ATP-SwRS-0778]
SlippingExcess，测得的加速度在项目配置范围内满足一定时间，是ATP判断过度空转的必要条件之一。
```
	def SlippingExcess(k):
	  if (MotionUnderEstimationState(k-1) is not SLIPPING):
	      slipping_excess_counter = 0
	      return False
	  elif (FilteredWheelAcceleration(k) < ATPsetting.SlippingStopAcc
	        and FilteredWheelAcceleration(k) > ATPsetting.SlidingStopAcc):
	      slipping_excess_counter(k) = slipping_excess_counter(k-1) + 1
	      return (slipping_excess_counter >= ATPsetting.SlippingExcessTime)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-
0148][End]
[iTC_CC_ATP-SwRS-0779]
The MotionUnderEstimationState transfers from SLIPPING to SKIDDING when:
```
	if (MotionUnderEstimationState(k-1) is SLIPPING
	     and OdometerState(k) is INITIALIZED
	     and OdometerAxleMotorized(k)
	     and (TimeInSlipping(k-1) > ATPsetting.SlippingTimeout
	          or (((StartSlippingSpeed(k-1) + TimeInSlipping(k-1)* ATPsetting.SlippingStopAcc)
	                <= WheelMinSpeed(k))
	               and SlippingExcess(k))))
	    MotionUnderEstimationState = SKIDDING
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-
0148][End]
[iTC_CC_ATP-SwRS-0794]
The MotionUnderEstimationState transfers from SKIDDING to COASTING when: 
```
	if (MotionUnderEstimationState(k-1) is SKIDDING
	     and (WheelFilteredStopped(k)
	           or OdometerState(k) is INVALID))
	    MotionUnderEstimationState = COASTING
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0148], [iTC_CC_ATP_SwHA-0084]
[End]
[iTC_CC_ATP-SwRS-0780]
StartMotoringMovementMin，记录由COASTING进入MOTORING，COASTING进入SLIPPING，或者MOTORING进入SLIPPING状态时的最小位移。
ATP records the minimum movement when the state transferring from COASTING to MOTORING or SLIPPING, or from MOTORING to SLIPPING.
```
	def StartMotoringMovementMin(k):
	    if (Initialization
	        or OdometerState(k-1) is not INITIALIZED
	        or (MotionUnderEstimationState(k) is COASTING)):
	        return 0
	    elif ((MotionUnderEstimationState(k-1) is COASTING
	             and MotionUnderEstimationState (k) is MOTORING)
	           or (MotionUnderEstimationState(k-1) is COASTING
	               and MotionUnderEstimationState (k) is SLIPPING)
	           or (MotionUnderEstimationState(k-1) is MOTORING
	                and MotionUnderEstimationState(k) is SLIPPING)):
	        return MinimumTrainMotion(k-1)
	    else:
	        return StartMotoringMovementMin(k-1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-
0148][End]
[iTC_CC_ATP-SwRS-0795]
UnderestimatedMotionMin，根据空转状态机，对里程计测得的最小位移进行补偿。
在MOTORING状态下，使用牵引入口位移和将测得位移低估15%补偿后二者较大的一个，作为补偿后的位移。
在SLIPPING状态下，使用牵引入口位移作为补偿后的位移。
```
	def UnderestimatedMotionMin(k):
	    if MotionUnderEstimationState(k) is MOTORING:
	        if (WheelMinimumMovement(k) >= 0):
	            return max(abs(StartMotoringMovementMin(k)),
	                       abs(WheelMinimumMovement(k) * ATPsetting.SlippingCoefficient))
	        else:
	            return -1 * max(abs(StartMotoringMovementMin(k)),
	                            abs(WheelMinimumMovement(k) * ATPsetting.SlippingCoefficient))
	    elif MotionUnderEstimationState(k) is SLIPPING:
	        return StartMotoringMovementMin(k)
	    else:
	        return WheelMinimumMovement(k)
```
\#Category=Functional
\#Contribution
=SIL4\#Allocation=ATP Software
, Vital Embedded Setting\#S
ource=[iTC_CC-SyAD-0148][End]
[iTC_CC_ATP-SwRS-0796]
UnderestimatedMotionMax，根据空转状态机，对里程计测得的最大位移进行补偿。
```
	def UnderestimatedMotionMax(k):
	    return WheelMaximumMovement(k)
```
\#Category=Functional
\#Contribution
=SIL4\#Allocation=ATP Software
\#S
ource=[iTC_CC-SyAD-0148][End]
