**********************************************************************************

INCC6 Beta Release 6
6.0.1.6 May 10, 2016 J. Longo

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
   SQL Server support is available.
   INCC6.exe.Config contains the connection details, manually configure following the notes
   and example in the file. Create the SQL Server database using the create_INCC6_sqlserver.sql
   script.
4. If for any reason the INCC6 Beta release stops working, you may opt to restore the
   default SQLite database by running the "reset.bat" batch file or
   by copying Default.sqlite over INCC6.sqlite. For SQL Server run the drop_INCC6_sqlserver.sql
   script followed by create_INCC6_sqlserver.sql.
   
KNOWN ISSUES:

Many dialogs and features from INCC 5.* are not fully implemented e.g.
   Reanalysis, Holdup and glovebox features, the Plot features,
   Deming integration, Collar item data entry,
   Collar acquire and Poison Rod details, integrated help.

This is beta release software; issues and missing features are known to exist.

CONTACT:

Please use direct email to j.longo@iaea.org or heather@lanl.gov for support and issue reporting.

Issues for basic INCC5/6 feature COMPLETION

#84*	Implement Collar
#76*	LM wizard gatewidth param change not used in subsequent live DAQ analysis
#75*	LM wizard setting for cycle count not used in Live DAQ
#41*	INCC6 CANNOT be KILLED!!! :)
#31*	Implement the INCC5 Reanalysis
#86+	List mode results database persistence 
#79+	Plot Norm, Plot Ver diagnostic tools
#78+	Find Deming curve fitting tool replacement
#69+	Present the proposed List Mode Config UI from DB enhancement
#66+ 	Delete measurements crashes the code 
#40+	source ID in 'initial source' does not get passed to results
#35+	Hold-up analysis required
#32+	Implement INCC5 File > Save As/Export > Transfer and Initial Data
#23+	how to delete list mode detector?

*HIGH Priority, +MEDIUM Priority

#67	PTR-32 data handling question 
#59	Toggle for Fast/Conventional on the Meas Params dlg for LM not working 
#45	calc_alpha_beta performance and efficiency
#43	Performing LM as SR after doing LM Acquire craps
#25	List mode acquisition predelay not stored
#16	Accidental Singles test Failure for PTR32
#13	PTR-32 -- Results different if reading file/doing live acquisition

**********************************************************************************

Includes code from the NDesk.Options library.

Author:
 Jonathan Pryor <jpryor@novell.com>

Copyright (C) 2008 Novell (http://www.novell.com)

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

**********************************************************************************

Includes code from the BigNum library.

Copyright Adam A. Brown, 2010
The BigNum library is the exclusive intellectual property of Adam Brown.
You may copy, modify and use this code for any purpose without prior authorization,
provided that you keep this attribution in the file, no matter what changes are made. 
check www.fractal-landscapes.co.uk for any updates. 

**********************************************************************************

