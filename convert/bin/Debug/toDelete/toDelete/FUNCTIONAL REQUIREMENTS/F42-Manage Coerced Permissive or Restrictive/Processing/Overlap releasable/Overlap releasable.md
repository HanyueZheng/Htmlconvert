
+Overlap状态管理
当选择BM模式，若OverlapTimer大于0，则设置OverlapTimerPermissive为True
若OverlapTimerPermissive为True，则所有信号机的overlap强制建立
Overlap timer管理
BM模式下，车头最大定位通过具有OverlapTimer初始化属性的信号机，则设置OverlapTimer为该信号机的变量有效期，并倒计时；当CC发送OverlapReleasable为允许状态时，并设置OverlapTimer为0；当CC未授权在BM下运行时，也设置OverlapTimer为0
[iTC_CC_ATP-SwRS-0600]
BMoverlapReleasableSendable，在BM下且未被ATC切除状态下，通过无线发给CI解锁信息。
```
	def BMoverlapReleasableSendable(k):
	    return Offline.GetBMoverlapReleasableSendable(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1313], [iTC_CC_ATP_SwHA-0253]
[End]
[iTC_CC_ATP-SwRS-0673]
OverlapReleasable，可发送Overlap解锁信息
```
	def OverlapReleasable(k):
	    return (BMoverlapReleasableSendable(k)
	            and TrainFilteredStopped(k)
	            and BlockModeEOAvalid(k)
	            and NonVitalRequest.OverlapRelease(k))
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1292], [iTC_CC_ATP_SwHA-0254]
[End]
[iTC_CC_ATP-SwRS-0674]
CrossedOverlapTimerInitialSignal，即本周期通过一个overlap timer初始化信号机时，返回该信号机奇点
```
	def CrossedOverlapTimerInitialSignal(k):
	    Signal = TrackMap.ExistSingBtwTwoLocs(SGL_SIGNAL, TrainFrontLocation(k-1).Max,
	                                                  TrainFrontLocation(k).Max)
	    if (Signal is not None
	        and Signal.BmOverlapTimerInit):
	        return Signal
	    else:
	        return None
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1189]
[End]
[iTC_CC_ATP-SwRS-0675]
OverlapTimer，当经过具有Overlap初始化属性的信号机时，将OverlapTimer设置为当时信号机的变量有效期。
```
	def OverlapTimer(k):
	    if (not BlockModeEOAvalid(k)
	        or OverlapReleasable(k)):
	        return 0
	    elif (BMvariantValidWhileTemporallyValid(k)
	          and CrossedOverlapTimerInitialSignal(k) is not None):
	        return BMvariantRemainingTime(CrossedOverlapTimerInitialSignal(k).CBIvariant.Id, k)
	    else:
	        return OverlapTimer(k-1) - 1
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1185], [iTC_CC-SyAD-1189], [iTC_CC-SyAD-1191], [iTC_CC-SyAD-1192], [iTC_CC_ATP_SwHA-0255]
[End]
[iTC_CC_ATP-SwRS-0676]
OverlapTimerPermissive，用于判断是否在BM下强制Overlap状态建立.
```
	def OverlapTimerPermissive(k):
	    return (OverlapTimer(k) > 0)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1185], [iTC_CC_ATP_SwHA-0255]
[End]
