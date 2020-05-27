
[iTC_CC_ATP-SwRS-0729]
CCvariantRequestMsgReceived，收到来自CI的CC变量请求并校核字正确。
```
	def CCvariantRequestMsgReceived(cbiId, k):
	    if (Initialization):
	        return False
	    elif (Message.Received(CCvariantRequest, k)):
	        return True
	    else:
	        return CCvariantRequestMsgReceived(Cbi, k-1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1193]
[End]
[iTC_CC_ATP-SwRS-0730]
ReceivedCBIloopHour，记录CBI请求中的联锁的loop hour。
```
	def ReceivedCBIloopHour(cbi, k):
	    if (Initialization):
	        return INVALID_LOOP_HOUR
	    elif (CCvariantRequestMsgReceived(Cbi, k)):
	        return CCvariantRequest(Cbi, k).CbiLoopHour
	    else:
	        return ReceivedCBIloopHour(Cbi, k-1)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1195]
[End]
