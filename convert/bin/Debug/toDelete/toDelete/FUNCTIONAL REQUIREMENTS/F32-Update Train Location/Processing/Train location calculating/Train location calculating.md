
[iTC_CC_ATP-SwRS-0283]
TrainLocalized，表示当前列车是否定位。
Only the localization state is LOCALIZED, ATP shall consider the train has localized.
```
	def TrainLocalized(k):
	    if (Initialization
	        or LocalizationFault(k)):
	        return False
	    elif (not TrainLocalized(k-1)
	          and (TrainPresumablyLocalized(k)
	               or TrainLocatedOnOtherATP(k)
	               or TrainLocatedOnBeacon(k))):
	        return True
	    else:
	        return TrainLocalized(k-1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0213], [iTC_CC-SyAD-0137]
[End]
当列车定位初始化后，ATP可根据里程计测得并经打滑补偿和轴锁判断处理的列车位移，每周期更新列车在线路地图中的位置。如果再读到信标，则ATP可根据该信标的位置对之前的定位进行重新校正。考虑到安全，ATP需维护列车每端车头的外侧和内侧两组定位信息。
When the train passed the continuous two beacons, ATP can judge the initial location and direction according to the position and the sequences of above-mentioned beacons in track map. Later, ATP can update the train location in the track map in each cycle based on the train movement combined with sliding overestimation and wheel block consideration. If ATP received a new beacon, it will realign the train location according to this beacon. For safety, ATP needs to maintain the location information from the external and internal side of each train 
end.
[iTC_CC_ATP-SwRS-0258]
TrainLocation，列车End1和End2端定位。
分为以下四种情况：
本周期非定位；
本周期刚初始化；
本周期经过信标重定位；
本周期使用位移累加定位。
```
	def TrainLocation(k):
	    if (not TrainLocalized(k)):
	        return None
	    elif (not TrainLocalized(k-1)):
	        return TrainInitialLocation(k)
	    elif (TrainRealignmentOnBeacon(k)):
	        return LocationAfterReloc(k)
	    else:
	        return LocationBeforeReloc(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0172], [iTC_CC-SyAD-0176], [iTC_CC-SyAD-0185], [iTC_CC-SyAD-0196], [iTC_CC_ATP_SwHA-0099]
[End]
