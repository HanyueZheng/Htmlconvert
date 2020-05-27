
[iTC_CC_ATP-SwRS-0649]
TrainInitialLocation，记录列车通过远端ATP、记忆定位、或信标初始化时的位置。
如果列车失位，则清除该位置
如果列车保持定位，则保留该位置
ATP determine the initial train location by redundant ATP, memorized location and beacon location in order. If train delocalized, the train location should be clear.
```
	def TrainInitialLocation(k):
	    if (TrainLocatedOnOtherATP(k)):
	        return OtherATP(k).Location
	    elif (TrainPresumablyLocalized(k)
	          and not TrainHasMoved(k)):
	        return MemLocation(k)
	    elif (TrainLocatedOnBeacon(k)):
	        return BeaconLocation(k)
	    elif (TrainLocalized(k-1)):
	        return TrainInitialLocation(k-1)
	    else:
	        return None
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1207], [iTC_CC-SyAD-1212], [iTC_CC_ATP_SwHA-0245]
[End]
