
[iTC_CC_ATP-SwRS-0045]
VLEimpulseNb[ATP_INTERRUPT_NB]，存储每次触发中断时获取的VLE脉冲数。ATP软件在每次响应安全时钟的硬件中断后，需通过VLE_GetSafeTime接口获取VLE的脉冲数，将其存储在VLEimpulseNb数组中。
VLEimpulseNb[ATP_INTERRUPT_NB] array stores the safe clock impulse number for every interrupt triggered. ATP shall obtain the impulse number through VLE_GetSafeTime, and stores into the array. 
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0012]
[End]
[iTC_CC_ATP-SwRS-0756]
ATP上电后，在每次响应中断后将ImmediateCounter加1，作为中断计数器。
ImmediateCounter, as the interrupt counter, ATP shall accumulate 1 after each interrupt triggered.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0012]
[End]
[iTC_CC_ATP-SwRS-0046]
定时中断被激活后，在每次中断中对ImmediateNb，CycleSynchronized，Trace，Dt和CycleBiasNb进行更新：
如果当前是上电后第一个中断：ATP需设置ImmediateNb为0，并根据所在的CPU初始化Trace，VCP的时间标签Dt，以及中间变量m：
```
	if (DataPlugContent.VLECpuId == CPU1)
	    Dt = CPU1_DT_INIT
	    Trace = CPU1_TRACE_0 ^ Dt
	    m = InversePDoperation(CPU1_TRACE_0, CPU1_TRACE_N)
	else:
	    Dt = CPU2_DT_INIT
	    Trace = CPU2_TRACE_0 ^ Dt
	    m = InversePDoperation(CPU2_TRACE_0, CPU2_TRACE_N)
```
否则，如果ImmediateNb = 0，而VitalTime与上个中断相比仍然未发生变化，则
将CycleBiasNb加1。
如果CycleBiasNb > 1，设置CycleSynchronized为False；
否则，如果ImmediateNb = 0，而VitalTime与上个中断相比发生了变化，则
令ImmediateNb = 1；
令Trace = PDoperationDt(Trace，Bi[ImmediateNb], Dt)
设置CycleBiasNb = 0，而且CycleSynchronized为True.
使用LockedImmediateCounter锁存此时的ImmediateCounter值，作为新周期初始的中断号。
否则，
将ImmediateNb的值加1；
令Trace = PDoperationDt(Trace, Bi[ImmediateNb], Dt)；
如果ImmediateNb > (ATP_INTERRUPT_NB-1)，则设置ImmediateNb = 0；并令Trace = PDoperation(Trace，m)，令Dt = PDoperation(Dt, 0)
When the fixed-time interrupt triggered, ATP shall update the ImmediateNb，CycleSynchronized, Trace, Dt and CycleBiasNb. 
If it is the first interrupt after powered up, ATP shall set ImmediateNb as zero，and initialize the Trace, Dt (the dynamic time of VCP), and the middle variables m based on CPU.
```
	if (DataPlugContent.VLECpuId == CPU1)
	    Dt = CPU1_DT_INIT
	    Trace = CPU1_TRACE_0 ^ Dt
	    m = InversePDoperation(CPU1_TRACE_0, CPU1_TRACE_N)
	else:
	    Dt = CPU2_DT_INIT
	    Trace = CPU2_TRACE_0 ^ Dt
	    m = InversePDoperation(CPU2_TRACE_0, CPU2_TRACE_N)
```
Or else:, If the ImmediateNb is zero, but the VitalTime has not changed comparing to the previous interrupt, then:
```
	CycleBiasNb = CycleBiasNb + 1
	if (CycleBiasNb > 1)
	    CycleSynchronized = False
```
Or else, If ImmediateNb is zero, and the VitalTime has changed comparing to the previous interrupt, then:
```
	ImmediateNb = 1
	Trace = PDoperationDt(Trace，Bi[ImmediateNb], Dt)
	CycleBiasNb = 0
	CycleSynchronized = True
	LockedImmediateCounter = ImmediateCounter
```
Otherwise, set:
```
	ImmediateNb = ImmediateNb + 1
	Trace = PDoperationDt(Trace, Bi[ImmediateNb], Dt)
```
and if the ImmediateNb is greater than (ATP_INTERRUPT_NB-1), then:
```
	ImmediateNb = 0
	Trace = PDoperation(Trace，m)
	Dt = PDoperation(Dt, 0)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0012], [iTC_CC_ATP_SwHA-0002], [iTC_CC_ATP_SwHA-0004]
[End]
NOTES：
其中CPU1_TRACE_0，CPU2_TRACE_0，CPU1_TRACE_N，CPU2_TRACE_N，CPU1_DT_INIT，CPU2_DT_INIT，以及数组Bi[ATP_INTERRUPT_NB]为VCP事先分配好的特征值。而PDoperation()表示不带Dt的PD运算；PdoperationDt()为带Dt的PD运算；InversePDoperation()为PD运算的逆运算。
The numbers of data is classified by the VCP tool beforehand which includes: CPU1_TRACE_0, CPU2_TRACE_0, CPU1_TRACE_N, CPU2_TRACE_N, CPU1_DT_INIT, CPU2_DT_INIT, and Bi[ATP_INTERRUPT_NB]. and PDoperation() represents the PD calculation without Dt；PDoperationDt() stands for the PD calculation with Dt ; InversePDoperation() is regarded as PD inverse calculation. 
