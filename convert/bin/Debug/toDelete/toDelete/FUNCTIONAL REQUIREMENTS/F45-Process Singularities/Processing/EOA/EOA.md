
CBTC模式下，授权行驶终点应作为ATP的限制点：
如果从列车车头最小定位到EB实际施加位置的范围内存在来自ZC的EOA点，ATP需将其作为安全速度限制区域处理，其限速值为0；
如果来自ZC的EOA位于EBA点下游，则ATP将其作为安全限速点，限速为0.
[iTC_CC_ATP-SwRS-0713]
ZoneVSLnotExceedEOA，CBTC下EOA作为区域型限速的情形
```
	def ZoneVSLnotExceedEOA(k):
	    if (CBTCmodeEOAvalid(k)
	        and (TrackMap.LocationBtwTwoLocs(CBTCmodeEOAlocation(k),
	                             TrainFrontLocation(k).Min,
	                             TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max, X2EbApplied(k))))):
	        return False
	    else:
	        return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0289], [iTC_CC-SyAD-0294], [iTC_CC-SyAD-0300], [iTC_CC_ATP_SwHA-0258]
[End]
[iTC_CC_ATP-SwRS-0714]
PointVSLnotExceedEOA，CBTC下EOA作为点型限速的情形
```
	def PointVSLnotExceedEOA(k):
	    if (CBTCmodeEOAvalid(k)
	        and not (TrackMap.LocationBtwTwoLocs(CBTCmodeEOAlocation(k),
	                              TrainFrontLocation(k).Min,
	                              TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max, X2EbApplied(k))))
	        and TrainEnergy(k) >= (Energy.AccumulationPotentialEnergy
	                                   (TrackMap.CalculateZoneBorder(TrainFrontEnd(k).Max,
	                                                                       X2EbApplied(k)),
	                                   CBTCmodeEOAlocation(k)))):
	        return False
	    else:
	        return True
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0289], [iTC_CC-SyAD-0294], [iTC_CC-SyAD-0300], [iTC_CC-SyAD-0311], [iTC_CC-SyAD-0314], [iTC_CC_ATP_SwHA-0258]
[End]
