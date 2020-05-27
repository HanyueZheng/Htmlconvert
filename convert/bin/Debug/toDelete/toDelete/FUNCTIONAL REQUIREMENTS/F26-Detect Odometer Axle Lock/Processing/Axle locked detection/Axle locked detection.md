
[iTC_CC_ATP-SwRS-0239]
AxlePossiblyLocked，在两路参考速度都正常（没有失效out of order）的情况下，当两路参考速度均判断本端里程计可能故障的情况下，认为当前可能轴锁。或者，当有一路参考速度认为轴锁，而另一路参考速度失效或不可用，也认为当前可能轴锁。
Odometer axle shall consider possibly locked if:
Both independent sources of odometry indicates a contradiction with local odometer,
Or one source of odometry is contradictory and the other one is out of order (or not available).
```
	AxlePossiblyLocked(k)
	 = ((OdometerRef_1.Contradictory(k) and OdometerRef_2.Contradictory(k))
	    or (OdometerRef_1.Contradictory(k)
	         and (OdometerRef_2.OutOfOrder(k) or  not ReferenceSpeedAvailable_2(k)))
	    or (OdometerRef_2.Contradictory(k)
	         and (OdometerRef_1.OutOfOrder(k) or not ReferenceSpeedAvailable_1(k))))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0136], [iTC_CC-SyAD-1156], [iTC_CC_ATP_SwHA-0091]
[End]
[iTC_CC_ATP-SwRS-0240]
UnrecoverableAxleLocked，当连续若干个周期判断可能轴锁，或者已经判断为轴锁，则永久轴锁.
If AxlePossiblyLocked situation lasts more than ATPsetting.OdoLockedAxleTimeout, the odometer axle shall be considered locked. Once UnrecoverableAxleLocked set to True, it will stay at state True unless ATP re-initialized.
```
	UnrecoverableAxleLocked(k)
	 = UnrecoverableAxleLocked(k-1)
	   or (AxlePossiblyLocked(k)
	        and AxlePossiblyLocked(k-1)
	        and ...
	        and AxlePossiblyLocked(k+1-ATPsetting.OdoLockedAxleTimeout)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0136], [iTC_CC-SyAD-0137], [iTC_CC-SyAD-0364], [iTC_CC_ATP_SwHA-0091]
[End]
[iTC_CC_ATP-SwRS-0241]
AxleLockedDetectionAvailable，只要有一路参考速度可以工作，就认为轴锁侦测可用。
If only one or no source of odometry is available, then ATP shall invalidate kinematic while this situation lasting.
```
	AxleLockedDetectionAvailable
	  = ((not OdometerRef_1.OutOfOrder and ReferenceSpeedAvailable_1(k))
	    or (not OdometerRef_2.OutOfOrder and ReferenceSpeedAvailable_2(k)))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0136], [iTC_CC-SyAD-1157], [iTC_CC_ATP_SwHA-0091], [iTC_CC_ATP_SwHA-0237], [iTC_CC_ATP_SwHA-0238]
[End]
[iTC_CC_ATP-SwRS-0242]
WheelTrainKinematicCorrelation，车轮和列车的速度一致性
Wheel and train kinematic shall consider correctly correlated if and only if:
odometer axle is not detected locked,
and odometer axle detection is available
```
	WheelTrainKinematicCorrelation(k)
	 = AxleLockedDetectionAvailable(k) and not UnrecoverableAxleLocked(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0136], [iTC_CC-SyAD-0364], [iTC_CC-SyAD-0137], [iTC_CC-SyAD-1157], [iTC_CC_ATP_SwHA-0238]
[End]
