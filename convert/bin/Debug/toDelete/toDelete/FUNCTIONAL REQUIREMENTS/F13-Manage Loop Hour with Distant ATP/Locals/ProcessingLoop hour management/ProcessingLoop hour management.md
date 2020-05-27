
ATP从上电开始，需根据自己的周期数和所在的列车车头，维护自身的loop hour，作为与外界通信的时间标签，用于监控通信的时效性。对于两端驾驶室的ATP所使用的初始以及最大时间均不同，且没有交集，即根据消息中的loop hour，也能分辨出该消息是来自或者发往哪一端驾驶室的ATP。
Since power up, ATP shall maintain its loop hour as a label used to monitor the timeline of communication, according to the cycle number and the cab where ATP settled. For both ends of the cab of the ATP, they use different loop hour initial value, and there is no intersection between the ranges. Thus, according to the message loop hour, the source of the message sent from which ATP can distinguish.
[iTC_CC_ATP-SwRS-0144]
ATPtime，维护本端ATP的loop hour时间。
根据本端CoreId，初始化为END_1或 END_2的初始值；
如果超过了相应的最大值，则重新等于初始化的值。
否则每周期加1
ATPtime stands for the ATP loop hour of this train END.
Based on CoreId, ATP initialize ATPtime as the initiative value of END_1 or END_2;
If the value exceeds the maximum loop hour, ATP shall set it as the initiative value; 
Otherwise, add one for each cycle. 
```
	def ATPtime(k):
	    if (CoreId(k) is END_1):
	        if (Initialization):
	            return CC1_INIT_TIME
	        elif (ATPtime(k-1) >= CC1_MAX_TIME):
	            return CC1_INIT_TIME
	        else:
	            return ATPtime(k-1) + 1
	    else:
	        if (Initialization):
	            return CC2_INIT_TIME
	        elif (ATPtime(k-1) >= CC2_MAX_TIME):
	            return CC2_INIT_TIME
	        else:
	            return ATPtime(k-1) + 1
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0208], [iTC_CC-SyAD-0209], [iTC_CC-SyAD-0221], [iTC_CC_ATP_SwHA-0016]
[End]
