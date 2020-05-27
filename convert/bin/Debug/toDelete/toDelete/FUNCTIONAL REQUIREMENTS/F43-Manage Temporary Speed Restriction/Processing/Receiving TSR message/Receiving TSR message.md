﻿
[iTC_CC_ATP-SwRS-0687]
TSRreportReceived，收到TSR消息
```
	def TSRreportReceived(lcId, k):
	    return Message.Received(TSRdownloadContent(lcId), k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0282]
[End]
[iTC_CC_ATP-SwRS-0099]
TSRreportAvailable，TSR消息可用
```
	def TSRreportAvailable(lcId, k):
	    return Message.Available(TSRreportReceived(lcId, k),
	                             TSRdownloadContent.CcLoopHour,
	                             ATPsetting.TSRvalidityTime,
	                             LastTSRreportAge(lcId, k-1),
	                             k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0282], [iTC_CC-SyAD-0391], [iTC_CC_ATP_SwHA-0019]
[End]
[iTC_CC_ATP-SwRS-0688]
LastTSRreportAge，记录当前使用的TSR消息已经过了多长时间。
```
	def LastTSRreportAge(lcId, k):
	    return Message.LastAge(TSRreportAvailable(lcId, k),
	                            TSRdownloadContent.CcLoopHour,
	                            LastTSRreportAge(lcId, k-1),
	                            k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0282]
[End]
NOTES：
由于TSR的解析和校核字计算需要一段时间，正常情况下，CCNV给LC消息发送的间隔应当大于TSR消息解析时间，确保在收到新消息时之前消息已解析完成。但如果在解析过程中又收到新的TSR消息时，应当遵循以下优先级处理：
应当继续解析当前的消息直至完成；
之后，选择与之前处理完成的消息所在不同的LC的消息进行解析；
对于每个LC里，只保留最新的一条消息，使用新消息覆盖旧的。