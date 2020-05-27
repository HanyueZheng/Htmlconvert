
[iTC_CC_ATP-SwRS-0446]
RTCtime，ATP维护的非安全时钟：
ATP软件在初始化时从VLE-2后板上获取RTC时钟信息；
之后，ATP软件每秒钟将该RTC时钟加1；
但如果RTC时钟与来自CCNV的NTP时间差超过MAX_NTP_TIME_ERROR，则使用NTP时间更新RTC时间。
ATP software shall maintain the RTC time for non-vital functions.
In initialization, ATP software get RTC time from VLE-2 board;
And then, ATP software updates the RTC time every second;
And if the difference between RTC time ATP used and the NTP time CCNV sent is greater than MAX_NTP_TIME_ERROR, ATP shall reset the RTC time as NTP time.
```
	if (Initialization)
	    RTCtime = VLE_RTCtime
	elif ((ATOcontrolTimeValid(k) == True)
	        and (NonVitalRequest.NtpTime != None)
	        and (|NonVitalRequest.NtpTime - RTCtime(k-1)| > MAX_NTP_TIME_ERROR))
	    RTCtime = NonVitalRequest.NtpTime
	else:
	    RTCtime = Time.Update()
```
其中Time.Update()意为ATP软件每秒钟将RTC时间加1。
The Time.Update() means ATP software shall update the RTC time every second.
\#Category=Functional
\#Contribution=SIL0
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0760], [iTC_CC-SyAD-0761], [iTC_CC-SyAD-1007]
[End]
