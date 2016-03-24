**********************************************************************************

INCC6 Beta Release 4
6.0.1.3 April 1, 2016 J. Longo

This work was supported by the United States Member State Support Program to IAEA Safeguards;
the U.S. Department of Energy, Office of Nonproliferation and National Security, International
Safeguards Division; and the U.S. Department of Energy, Office of Safeguards and Security.

Copyright 2016, Los Alamos National Security, LLC. This software application and associated
material ("The Software") was prepared by the Los Alamos National Security, LLC. (LANS), under
Contract DE-AC52-06NA25396 with the U.S. Department of Energy (DOE). All rights in the software
application and associated material are reserved by DOE on behalf of the Government and LANS
pursuant to the contract.

Redistribution and use in source and binary forms, with or without modification, are permitted provided
that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and
the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and
the following disclaimer in the documentation and/or other materials provided with the distribution.
3. Neither the name of the "Los Alamos National Security, LLC." nor the names of its contributors may
be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE REGENTS AND CONTRIBUTORS "AS IS" AND ANY
EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR THE CONTRIBUTORS BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSITITUTE GOODS OR
SERVICES; LOSS OF USE, DATA OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED OR ON ANY THEORY OF LIABILITY, WHETHER IN CONTRAT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE
USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.


**********************************************************************************

INSTALLATION INSTRUCTIONS:

1. Extract the contents of the incc.zip folder onto your PC.
2. Run INCC6.exe
3. A "bare" SQLite database has been provided for your use. It is named "incc6.sqlite".
   A SQL Server instance is available.
   INCC6.exe.Config contains the connection details, manually configure following the notes
   and example in the file. Create the SQL Server database using the create_INCC6_sqlserver.sql
   script.
4. If for any reason INCC6 Beta Release stops working, you may opt to restore the
   default SQLite database by running the "reset.bat" batch file or
   by copying Default.sqlite over INCC6.sqlite. For SQL Server run the drop_INCC6_sqlserver.sql
   script followed by create_INCC6_sqlserver.sql.
   
KNOWN ISSUES:

Many dialogs and features from INCC 5.* are not fully implemented e.g.
   Reanalysis, Holdup acquire, Glovebox entry, the Measurement, Verification and Holdup
   Summary reports, the Plot features, Deming integration, Collar item data entry,
   Collar acquire and Poison Rod details, integrated help.

MCA-527 list mode file processing is implemented, but live data collection support is not.

This is beta release software, and issues and missing features are known to exist.

CONTACT:

Please use direct email to j.longo@iaea.org or heather@lanl.gov for support and issue reporting.

Issues for basic feature COMPLETION

#70* 		Add MCA-527 single channel list mode DAQ
#41*		INCC6 CANNOT be KILLED!!! :)
#11*		Implement progress and cancel features for lengthy operations
#76* 		LM wizard gatewidth param change not used in subsequent live DAQ analysis
#75* 		LM wizard setting for cycle count not used in Live DAQ
#80* 		Summary results CSV reports
#31*		Implement the INCC5 Reanalysis
#84*		Implement Collar


#32		Implement INCC5 File > Save As/Export > Transfer and Initial Data
#79		Plot Norm, Plot Ver diagnostic tools
#78		Find Deming curve fitting tool replacement
#40		source ID in 'initial source' does not get passed to results
#35		Hold-up analysis required

*HIGH PRIORITY and these also

#67		PTR-32 data handling question 
#59		Toggle for Fast/Conventional on the Meas Params dlg for LM not working 
#45		calc_alpha_beta performance and efficiency
#43		Performing LM as SR after doing LM Acquire craps
#37		Urgent labels and others urgently may or may not need attention 
#25		List mode acquisition predelay not stored
#16		Accidental Singles test Failure for PTR32
#13		PTR-32 -- Results different if reading file/doing live acquisition




   
