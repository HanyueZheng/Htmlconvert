
ATP根据项目配置参数，在指定端口获取司机激活的驾驶室状态，据此来判断车头方向。如果司机未激活任意一端驾驶室，则ATP应当根据CCNV送来的驾驶室进行车头判断。
ATP shall determine the train front by cab activation from project configured VIOM input port. If the driver did not activate either end of the cab, ATP shall judge the train front according to the info from CCNV.
[iTC_CC_ATP-SwRS-0076]
DriverInCab_1或DriverInCab_2，如果采集到某端的驾驶室被激活，则ATP认为司机在该端驾驶室。其状态来自于项目可配置的列车输入采集。
ATP shall consider the driver is in this cab if it captures that either end of cab activated, which shown by the data from DriverInCab_1 or DriverInCab_2.
```
	def DriverInCab_1(k):
	    return Offline.GetDriverInCab_1(k)
```
```
	def DriverInCab_2(k):
	    return Offline.GetDriverInCab_2(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0346], [iTC_CC-SyAD-1003], [iTC_CC_ATP_SwHA-0190]
[End]
[iTC_CC_ATP-SwRS-0139]
DriverInTrain，当前是否有司机在车内
If the active status is different between two ENDs of the train, ATP consider there is a driver in train.
```
	def DriverInTrain(k):
	    return (DriverInCab_1(k) is not DriverInCab_2(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source= [iTC_CC-SyAD-0348], [iTC_CC_ATP_SwHA-0038]
[End]
[iTC_CC_ATP-SwRS-0127]
NonVitalSelectedFrontEnd，来自CCNV的车头选择信息
NonVitalSelectedFrontEnd represents the train front choice from CCNV. 
```
	if (ATOcontrolTimeValid(k) == True)
	    NonVitalSelectedFrontEnd = NonVitalRequest.SelectedFrontEnd(k)
	else:
	    NonVitalSelectedFrontEnd = UNKNOW
```
\#Category= Functional
\#Contribution=SIL0
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1044]
[End]
[iTC_CC_ATP-SwRS-0138]
TrainFrontEnd，判断司机在END_1还是END_2还是由CCNV选择。
If there is a driver in the train, the train front is the activated END. or else: the front determined by CCNV.
 Otherwise, the train front is the default one or the front one when train is moving.
```
	def TrainFrontEnd(k):
	    if (Initialization):
	       return END_2
	    elif (DriverInTrain(k)):
	        if (DriverInCab_1(k)):
	            return END_1
	        else:
	            return END_2
	    elif (NonVitalSelectedFrontEnd(k) is END_1
	          or NonVitalSelectedFrontEnd(k) is END_2):
	        return NonVitalSelectedFrontEnd(k)
	    elif (WheelFilteredStopped(k)):
	        return TrainFrontEnd(k-1)
	    elif (not End2RunningForward(k)):
	        return END_1
	    else:
	        return END_2
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0203], [iTC_CC-SyAD-0204], [iTC_CC-SyAD-0345], [iTC_CC-SyAD-1176], [iTC_CC_ATP_SwHA-0191]
[End]
[iTC_CC_ATP-SwRS-0281]
TrainFrontOrientation，列车运营方向.
The train front orientation is the orientation of the active train END.
```
	def TrainFrontOrientation(k):
	    if (TrainFrontEnd(k) is END_2):
	        return TrainLocation.Ext2.Ort(k)
	    else:
	        return TrainLocation.Ext1.Ort(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source= [iTC_CC-SyAD-0177], [iTC_CC-SyAD-0211]
[End]
[iTC_CC_ATP-SwRS-0249]
TrainFrontLocation，车头定位的更新:
ATP updates the train front location according to the active train END.
```
	def TrainFrontLocation(k):
	    if (TrainFrontEnd(k) is END_1):
	        TrainFrontLocation.Max = TrainLocation.Ext1
	        TrainFrontLocation.Min = TrainLocation.Int1
	    else:
	        TrainFrontLocation.Max = TrainLocation.Ext2
	        TrainFrontLocation.Min = TrainLocation.Int2
	    return TrainFrontLocation
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0211]
[End]
[iTC_CC_ATP-SwRS-0255]
TrainRearLocation，车尾定位的更新:
ATP updates the train rear locations according to the active train END.
```
	def TrainRearLocation(k):
	    if (TrainFrontEnd(k) is END_1):
	        TrainRearLocation.Max = TrainLocation.Int2
	        TrainRearLocation.Min = TrainLocation.Ext2
	    else:
	        TrainRearLocation.Max = TrainLocation.Int1
	        TrainRearLocation.Min = TrainLocation.Ext1
	    return TrainRearLocation
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0211]
[End]
