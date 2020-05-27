
[iTC_CC_ATP-SwRS-0325]
TrainPossiblyInOverEnergy，列车能量大于限制点或限制区能量，即超能。
If the train energy exceeds the zone of point vital speed limitation, ATP shall consider the train possibly over energy.
```
	def TrainPossiblyInOverEnergy(k):
	    return (not ZoneVSLNotExceed(k)
	        or not PointVSLNotExceed(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0316], [iTC_CC-SyAD-0227], [iTC_CC-SyAD-0276], [iTC_CC-SyAD-0137]
[End]
[iTC_CC_ATP-SwRS-0326]
TrainEnergyControlDisabled，在RM模式下不报超能。
If the RMF or RMR mode selected, ATP shall not monitor the train energy.
```
	def TrainEnergyControlDisabled(k):
	    return MotionProtectionInhibition(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0227], [iTC_CC-SyAD-0344], [iTC_CC-SyAD-0796], [iTC_CC-SyAD-0913], [iTC_CC-SyAD-1398]
[End]
[iTC_CC_ATP-SwRS-0327]
EBforOverEnergy，超能后是否输出EB
ATP shall request emergency braking if train is possibly in over-energy and train speed control enabled and if following conditions fulfilled:
the train is not detected at filtered stop,
or the train is detected at filtered stop and:
safe immobilization customization setting for this control indicates to use emergency brake,
or safe immobilization customization setting for this control indicates to use emergency brake when it was already applied.
```
	def EBforOverEnergy(k):
	    return (TrainPossiblyInOverEnergy(k)
	            and not TrainEnergyControlDisabled(k)
	            and (not TrainFilteredStopped(k)
	                 or (TrainFilteredStopped(k)
	                     and (ATPsetting.MPauthImmoBehaviourAtFS
	                          is IB_APPLY_EMERGENCY_BRAKE)
	                     or ((ATPsetting.MPauthImmoBehaviourAtFS
	                          is IB_APPLY_EMERGENCY_BRAKE_WHEN_TRIGGERED)
	                         and not InhibitEmergencyBrake(k-1)))))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0227], [iTC_CC-SyAD-0276], [iTC_CC-SyAD-0316], [iTC_CC-SyAD-0317], [iTC_CC_ATP_SwHA-0134], [iTC_CC-SyAD-0137]
[End]
[iTC_CC_ATP-SwRS-0328]
PBforOverEnergy，超能停车后是否继续输出PB
ATP shall request parking braking if train is possibly in over-energy and train speed control enabled and if following conditions fulfilled:
the train is detected at filtered stop,
and safe immobilization customization setting for this control indicates to use parking brake.
```
	def PBforOverEnergy(k):
	    return (TrainPossiblyInOverEnergy(k)
	            and not TrainEnergyControlDisabled
	            and (TrainFilteredStopped(k)
	                 and (ATPsetting.MPauthImmoBehaviourAtFS is IB_APPLY_PARKING_BRAKE))) 
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0230], , [iTC_CC-SyAD-0227], [iTC_CC-SyAD-0231], [iTC_CC-SyAD-0317], [iTC_CC_ATP_SwHA-0183]
[End]
