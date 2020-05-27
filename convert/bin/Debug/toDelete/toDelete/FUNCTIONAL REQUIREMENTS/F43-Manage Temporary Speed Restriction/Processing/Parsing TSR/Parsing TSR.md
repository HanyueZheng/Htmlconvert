
NOTES:
根据[REF5]，LC发送的TSR消息按照ST_TSR_DOWN_CONTENT组织，但进行存储和SACEM校核时，需按照ST_TSR_BLOCK格式映射到线路地图上的每个BLOCK中。
对于每条TSR消息的中间BLOCK，其最小最大坐标分别为0和BLOCK长度值；
对于每条TSR消息的起始BLOCK：
如果该TSR区域是按照UP方向设置的，则转换后该BLOCK的最小坐标对应TSR的起始坐标；而最大坐标对应为该BLOCK长度值，或者TSR结束坐标（如果该TSR区域只包括这一个BLOCK）；
如果该TSR区域是按照DOWN方向设置的，则转换后该BLOCK的最小坐标对应为0，或TSR的结束坐标（如果该TSR区域只包括这一个BLOCK）；而最大坐标对应TSR的起始坐标。
对于每条TSR消息的结束BLOCK：
如果该TSR区域是按照UP方向设置的，则转换后该BLOCK的最小坐标对应为0，或TSR的起始坐标（如果该TSR区域只包括这一个BLOCK）；而最大坐标对应TSR的结束坐标。
如果该TSR区域是按照DOWN方向设置的，则转换后该BLOCK的最小坐标对应TSR的结束坐标；而最大坐标对应为该BLOCK长度值，或者TSR起始坐标（如果该TSR区域只包括这一个BLOCK）。
[iTC_CC_ATP-SwRS-0102]
ReceivedTSRdatabase，将LC发送的TSR消息报文映射到BLOCK数组中。对于线路上的每个BLOCK，判断其是否有对应的TSR，若有，则更新其首末点坐标和限速值，其中需将TSR消息中的坐标和速度单位转化为ATP软件使用的坐标和速度单位。
ATP shall map the TSR message received from LC to structure of block. It need to judge whether there is corresponding TSR for each BLOCK in the track map. If yes, ATP shall update the abscissa of the starting and ending points, as well as the restriction speed. During the process, it need to transfer the abscissa and speed unit of TSR message to the corresponding one used in ATP.

|Identification|Logical Type|Description|
|-----|
|ReceivedTSRdatabase|||
||ValidityTime| REF NUMERIC_32 \h  \* MERGEFORMAT NUMERIC_32|Expiration time for the TSR message|
||Blocks[ REF MAX_BLOCK_NB \h MAX_BLOCK_NB]| REF ST_TSR_BLOCK \h ST_TSR_BLOCK|TSR for each block|

```
	def ReceivedTSRdatabase(lc, k):
	    if (Initialization
	        or (Message.Exists(DateSynchronizationReport(lc), k)
	            and Message.Exists(VersionAuthorization(lc), k)
	            and not Message.Exists(TSRdownloadContent(lc), k))
	        or (not Message.IsMoreRecent(ReceivedTSRdatabase(lc, k-1).ValidityTime, ATPtime(k))
	            and not TSRreportAvailable(k))):
	            S
	etAllBlockAsDefaultTsr    elif (TSRreportAvailable(lc, k)):
	        NewValidity = 0
	        if (Message.ReplyLocalCC(TSRdownloadContent(lc).CcLoopHour)):
	            NewValidity = (TSRdownloadContent(lc).CcLoopHour + ATPsetting.TSRvalidityTime)
	        else:
	            NewValidity = (ATPtime(k) + ATPsetting.TSRvalidityTime
	                            - (OtherATPmaxTime(k) - TSRdownloadContent(lc).CcLoopHour))
	        ReceivedTSRdatabase.ValidityTime = NewValidity
	        for tsr in range(0, TSRdownloadContent(lc).NumberOfTsr):
	            S
	etTsrInFirstBlock            S
	etTsrInLastBlock            for blk in range(TSRdownloadContent(lc).Tsr[tsr].FirstBlockId + 1,
	                              TSRdownloadContent(lc).Tsr[tsr].LastBlockId):
	                S
	etTsrInIntermediateBlock    else:
	        ReceivedTSRdatabase = ReceivedTSRdatabase(lc, k-1)
	    return ReceivedTSRdatabase
```
其中，SetAllBlockAsDefaultTsr表示将线路所有该LC管辖的Block均设置为默认的TSR限速值ATPsetting.TSRdefaultLimitSpeed； 
SetTsrInFirstBlock表示TSR消息中首个Block的TSR设置；
SetTsrInIntermediateBlock表示TSR消息里中间Block的TSR设置。
SetTsrInLastBlock表示TSR消息中末尾Block的TSR设置。
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0281], [iTC_CC-SyAD-0283], [iTC_CC-SyAD-0390], [iTC_CC-SyAD-0392], [iTC_CC-SyAD-0393], [iTC_CC-SyAD-0914], [iTC_CC-SyAD-1005], [iTC_CC_ATP_SwHA-0022], [iTC_CC_ATP_SwHA-0177], [iTC_CC_ATP_SwHA-0178]
[End]
NOTES：
对于TSR的处理，仅支持一个BLOCK上至多有一个TSR的情况，其开始和结束点可以在该BLOCK上的任何位置。不支持一个BLOCK上有多个TSR。
For TSR processing, the iTC system supports only one TSR at one BLOCK at most. The beginning and the termination point of the TSR can be set any position in this block. However, it never sustains the situation that there are more than one TSR in one block.
