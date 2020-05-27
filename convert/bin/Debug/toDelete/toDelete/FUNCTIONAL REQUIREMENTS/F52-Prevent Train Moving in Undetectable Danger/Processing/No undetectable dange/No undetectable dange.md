
在未建立ATB模式的情况下，ATP应当避免列车处于“无法侦测的危险”的风险中，即列车向司机未授权的方向运行。在此种情况下，如果列车运行超过项目配置的距离，则ATP应当触发EB，并保持一段可配置的时间。
ATP shall avoid the train in "undetectable danger risk", which means that the train runs on the direction not authorized by the driver or by CCNV in ATB driving mode. In this case, if the train runs more than a configurable distance, ATP shall trigger emergency brake and keep it for a configurable period.
[iTC_CC_ATP-SwRS-0582]
NoUndetectableDanger_1，已监控向END_1方向的运行，其状态来自于项目可配置的列车输入采集。
The No Undetectable Danger in Extremity 1 shall be consider as permissive status according to project configuration.
```
	def NoUndetectableDanger_1(k):
	    return Offline.NoUndetectableDanger_1(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source= [iTC_CC-SyAD-0351], [iTC_CC-SyAD-0971],[iTC_CC-SyAD-1036], [iTC_CC_ATP_SwHA-0215], [iTC_CC-SyAD-1352], [iTC_CC_ATP_SwHA-0265]
[End]
[iTC_CC_ATP-SwRS-0583]
NoUndetectableDanger_2，其状态来自于项目可配置的列车输入采集。
The "No Undetectable Danger in Extremity 2" shall be consider as permissive status according to project configuration.
```
	def NoUndetectableDanger_2(k):
	    return Offline.NoUndetectableDanger_2(k) 
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source= [iTC_CC-SyAD-0351], [iTC_CC-SyAD-0971], [iTC_CC-SyAD-1036], [iTC_CC_ATP_SwHA-0215], [iTC_CC-SyAD-1352], [iTC_CC_ATP_SwHA-0265]
[End]
NOTES：
在离线数据中，可将PermissiveZoneLogicalInput或NotRestrictiveZoneLogicalInput作为配置数据NoUndetectableDanger_1或NoUndetectableDanger_2的输入变量参与运算，即结合线路上vital zone及其变量的设置，实现NUDE的监控。
[iTC_CC_ATP-SwRS-0285]
UndetectableDangerRiskForNoNUDE，当前两端车头都没有NUDE输入，则认为列车存在“无法侦测的风险”。
If there is neither No Undetectable Danger in Extremity 1 nor No Undetectable Danger in Extremity 2 inputs, ATP shall consider the train is possible under the risk of undetectable danger.
```
	def UndetectableDangerRiskForNoNUDE(k):
	    return (not NoUndetectableDanger_1(k)
	             and not NoUndetectableDanger_2(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0349], [iTC_CC-SyAD-0350], [iTC_CC-SyAD-0351], [iTC_CC-SyAD-0354], [iTC_CC-SyAD-1320], [iTC_CC_ATP_SwHA-0215]
[End]
[iTC_CC_ATP-SwRS-0286]
PBforUndetectableDangerRisk，当停车且存在“无法侦测的风险”时，如果项目配置为输出停车制动，则ATP应当输出停车制动。
ATP shall request a parking braking if the possibility of an undetected danger has proven to be and if following conditions are fulfilled:
the train is detected at filtered stop,
safe immobilization customization setting for this control indicates to use parking brake.
```
	PBforUndetectableDangerRisk(k)
	 = (UndetectableDangerRiskForNoNUDE(k)
	    and TrainFilteredStopped(k)
	    and (ATPsetting.NUDEimmoBehaviourAtFS == IB_APPLY_PARKING_BRAKE))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0354], [iTC_CC-SyAD-0355], [iTC_CC_ATP_SwHA-0113]
[End]
[iTC_CC_ATP-SwRS-0287]
NUDEdistanceAccount_1，监控当司机未授权向END_1方向运行时，列车向END_1方向运行的距离，该值为非负数，
若在初始化阶段，或NUDE1为True，或已经EB并停车，则等于0；
否则，当测速无效时，将其设置为默认值
否则，当里程计已初始化后：
如果MaximumTrainMotion大于0，则等于上周期累加距离加上本周期最大位移，最小取0。
而如果MaximumTrainMotion小于等于0，则使用上周期值加最小位移（实际上就是减小该累加值，倒车），最小取0
否则，保持累加距离不变。
When the driver does not authorize the train running toward the END_1, ATP shall accumulate the distance of the train running toward to the END_1.
If in initialization, or the NoUndetectableDanger_1 is True, or the train has triggered EB and has stopped, ATP set this distance to 0;
Else if train kinematic has invalid, ATP set this distance to the default value.
Else if the odometer has initialized:
If the MaximumTrainMotionis greater than 0, ATP accumulate the maximum movement in this cycle with the distance of last cycle;
Or if the MaximumTrainMotionis less than or equal to 0, ATP use the minimum movement of this cycle plus to the distance last cycle (in fact, decrease the accumulated distance). The minimum of this accumulated distance is 0.
Otherwise, keep the distance unchanged.
```
	def NUDEdistanceAccount_1(k):    
	    if (INTIALIZATION
	         or NoUndetectableDanger_1(k)
	         or (EBappliedForMotionWithoutNUDE(k-1) and TrainFilteredStopped(k)))
	        return 0
	    elif (ValidTrainKinematic(k) != True)
	        return ATPsetting.NUDEdistanceWithoutMotionAvailable
	    elif (OdometerState(k) is INITIALIZED)
	        if (End1RunningForward(k))
	             return max(0, NUDEdistanceAccount_1(k-1)+ MaximumTrainMotion(k))
	        else:
	             return max(0, NUDEdistanceAccount_1(k-1)+ MinimumTrainMotion(k))
	    else:
	        return  NUDEdistanceAccount_1(k-1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0352], [iTC_CC-SyAD-0353], [iTC_CC-SyAD-1319], [iTC_CC_ATP_SwHA-0216]
[End]
[iTC_CC_ATP-SwRS-0288]
NUDEdistanceAccount_2，监控当司机未授权向END_2方向运行时，列车向END_2方向运行的距离，该值为非正数，
若在初始化阶段，或NUDE2为True，或已经EB并停车，则等于0；
否则，当测速无效时，将其设置为默认值；
否则，当里程计已经初始化后：
若 MaximumTrainMotion小于0，则等于上周期累加距离加上本周期最大位移，最大取0。
若MaximumTrainMotion大于等于0，则使用上周期值加最小位移，最大取0。
否则，保持累加距离不变。
When the driver does not authorize the train running toward the END_2, ATP shall accumulate the distance of the train running toward to the END_2.
If in initialization, or the NoUndetectableDanger_2 is True, or the train has triggered EB and has stopped, ATP set this distance to 0;
Else if train kinematic has invalid, ATP set this distance to the default value.
Else if the odometer has initialized:
if the MaximumTrainMotion is less than 0, ATP accumulate the maximum movement in this cycle with the distance of last cycle;
Else: if the MaximumTrainMotion is greater than or equal to 0, ATP use the minimum movement of this cycle plus to the distance last cycle (in fact, decrease the accumulated distance). The minimum of this accumulated distance is 0.
Otherwise, keep the distance unchanged.
```
	def NUDEdistanceAccount_2(k):
	    if (INTIALIZATION
	         or NoUndetectableDanger_2(k)
	         or (EBappliedForMotionWithoutNUDE(k-1) and TrainFilteredStopped(k)))
	        return 0
	    elif (ValidTrainKinematic(k) != True)
	        return -1 * ATPsetting.NUDEdistanceWithoutMotionAvailable(k)
	    elif (OdometerState(k) is INITIALIZED)
	        if (End2RunningForward(k))
	             return min(0, NUDEdistanceAccount_2(k-1)+ MaximumTrainMotion(k))
	        else:
	             return min(0, NUDEdistanceAccount_2(k-1)+ MinimumTrainMotion(k))
	    else:
	        return NUDEdistanceAccount_2(k-1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0352], [iTC_CC-SyAD-0353], [iTC_CC-SyAD-1319], [iTC_CC_ATP_SwHA-0216]
[End]
[iTC_CC_ATP-SwRS-0289]
UndetectDangerMotionWithoutNUDE，列车运行超过限定距离，但仍有车头未检测到NUDE。
When the train has moved without NUDE more than project-restricted distance, ATP shall set this value to True.
```
	UndetectDangerMotionWithoutNUDE(k)
	 = (not NoUndetectableDanger_1(k)
	       and (NUDEdistanceAccount_1(k)> ATPsetting.NUDElimitDistance))
	    or (not NoUndetectableDanger_2(k)
	         and (NUDEdistanceAccount_2(k)< -1 * ATPsetting.NUDElimitDistance)))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0352], [iTC_CC_ATP_SwHA-0114]
[End]
[iTC_CC_ATP-SwRS-0290]
EBappliedForMotionWithoutNUDE，保证由NUDE导致的EB会延迟一段时间。即：
当UndetectDangerMotionWithoutNUDE为True时，设置EBappliedForMotionWithoutNUDE为True；
当UndetectDangerMotionWithoutNUDE由True变为False后，还需保持EBappliedForMotionWithoutNUDE 在ATPsetting.NUDEtrainStopDurationBeforeEBrelease时间内为True；
超过上述时间后，该值为False。
The EB request shall be maintained to True during the application time ATPsetting.NUDEtrainStopDurationBeforeEBrelease, if the train has moved without NUDE more than project restricted distance.
 When UndetectDangerMotionWithoutNUDE is True, ATP shall set EBappliedForMotionWithoutNUDE to True;
 When UndetectDangerMotionWithoutNUDE change from True to False,  ATP shall maintain EBappliedForMotionWithoutNUDE to True in period ATPsetting.NUDEtrainStopDurationBeforeEBrelease；
Over the time, set this value to False.
```
	def EBappliedForMotionWithoutNUDE(k):
	    if (UndetectDangerMotionWithoutNUDE(k)):
	        NudeEBreleaseCounter = 0
	        return True
	    elif (NudeEBreleaseCounter < ATPsettings.NUDEtrainStopDurationBeforeEBrelease):
	        NudeEBreleaseCounter(k) = NudeEBreleaseCounter(k-1) + 1
	        return True
	    else:
	        return False
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0352], [iTC_CC-SyAD-0355], [iTC_CC-SyAD-1320], [iTC_CC_ATP_SwHA-0114]
[End]
[iTC_CC_ATP-SwRS-0291]
EBforUndetectableDangerRisk，由“无法侦测的危险”导致EB并停车后，ATP应当根据项目配置判断是否输出EB。
When the train has triggered emergency brake causing by the "undetectable danger risk" and has stopped, ATP shall determine whether keeping the EB output according to the project configuration.
```
	EBforUndetectableDangerRisk(k)
	 = (EBappliedForMotionWithoutNUDE(k)
	   or (UndetectableDangerRiskForNoNUDE(k)
	       and (not TrainFilteredStopped(k)
	            or (ATPsetting.NUDEimmoBehaviourAtFS == IB_APPLY_EMERGENCY_BRAKE)
	            or ((ATPsetting.NUDEimmoBehaviourAtFS == IB_APPLY_EMERGENCY_BRAKE_WHEN_TRIGGERED)
	                  and (not InhibitEmergencyBrake(k-1))))))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0352], [iTC_CC-SyAD-0355], [iTC_CC-SyAD-1320], [iTC_CC_ATP_SwHA-0115]
[End]
