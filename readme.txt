**********************************************************************************

INCC6 Beta Release 16
6.0.1.16 Sep 21, 2016 (NDATest)

This work was supported by the United States Member State Support Program to IAEA Safeguards;
the U.S. Department of Energy, Office of Nonproliferation and National Security, International
Safeguards Division; and the U.S. Department of Energy, Office of Safeguards and Security.

Copyright 2016, Los Alamos National Security, LLC. This software application and associated
material ("The Software") was prepared by the Los Alamos National Security, LLC. (LANS), under
Contract DE-AC52-06NA25396 with the U.S. Department of Energy (DOE). All rights in the software
application and associated material are reserved by DOE on behalf of the Government and LANS
pursuant to the contract.

Redistribution and use in source and binary forms, with or without modification, are permitted
provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions
   and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions
   and the following disclaimer in the documentation and/or other materials provided with the
   distribution.
3. Neither the name of the "Los Alamos National Security, LLC." nor the names of its contributors
   may be used to endorse or promote products derived from this software without specific prior
   written permission.

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
Minimum requirements

OS Windows 7 or above
.NET 4.5 or higher

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

**********************************************************************************

These are the basic files installed with INCC6.

All these files must reside in the same folder, but for the database.
The database may be stored elsewhere, with the path and file name stored in the .Config files.  


INCC6.exe        ; User Interface and starting point for INCC6
INCC6.exe.Config ; .NET config file for GUI INCC6. Current database is specified here

INCCCmd.exe         ; Command line entry point for INCC batch processing, also seen as INCC.exe
INCCCmd.exe.Config  ; .NET config file for command line. Current database is specified here

Defs.dll         ; Data and utility classes
RepDB.dll        ; Database API, report generator, config file and command line processing
NCCCtrl.dll      ; LMMM, PTR-32, MCA-527, SR instrument and file processing classes, list mode statistical counting

INCC6.sqlite     ; Database for INCC6, created with reset.bat (see below), referenced in both .Config files

SR32.dll         ; C++ DLL wrapper over LANL's INCC Shift Register protocol SR32.lib library (x86 only, no source code)   
LMSR.dll         ; C# .NET wrapper over SR32.dll (no source code)
NCCCore.dll      ; Statistical tests and Pu mass calculations (no source code)

inccuser.pdf     ; INCC 5 User Guide, has algorithms, theoretical equations, and more

SQLite/SQL Server database schema and related files

System.Data.SQLite.dll    ; SQLite .NET Interop assembly 1.0.94.0
sqlite3.exe               ; SQLite database utility, for diagnostic use

INCC6.sqlite              ; Empty database, created with reset.bat
Default.sqlite            ; Empty database, created with reset.bat
drop_INCC6_sqlite.sql     ; INCC6 SQLite schema DROP table statements
create_INCC6_sqlite.sql   ; INCC6 SQLite schema CREATE and INSERT statements
reset.bat                 ; Uses sqlite3.exe on sql schema files to recreate a new empty INCC6 database

drop_INCC6_sqlserver.sql   ; INCC6 SQL Server schema DROP table statements
create_INCC6_sqlserver.sql ; INCC6 SQL Server schema CREATE and INSERT statements
   
**********************************************************************************
KNOWN ISSUES:

Many dialogs and features from INCC 5.* are not fully implemented e.g.
   Holdup and glovebox features, the Plot features,
   some Add-a-src processing, Collar acquire and Poison Rod details,
   integrated help.

This is beta release software; issues and missing features are known to exist.

CONTACT:

Please use direct email to j.longo@iaea.org or heather@lanl.gov for support and issue reporting.

ISSUES
See https://github.com/hnordquist/INCC6/issues


Issues for basic INCC5/6 feature COMPLETION

#84  Implement Collar
#79  Plot Norm, Plot Ver diagnostic tools
#35  Hold-up analysis required

Issues for basic INCC6 function and performance

#100 Rossi processing too slow and can sometimes hang
#67  PTR-32 data handling question 
#59  Toggle for Fast/Conventional on the Meas Params dlg for LM not working 
#45  calc_alpha_beta performance and efficiency
#16  Accidental Singles test Failure for PTR32
#13  PTR-32 -- Results different if reading file/doing live acquisition

Closed issues

6.0.1.16 Sep 16, 2016
***  Issue with empty mtl type on collar params seen
***  Reanalysis now uses stored List Mode SR params
***  Current detector and related acquire state sometimes not restored
***  Cosmetic improvement to Acquire dialog source selection for list mode devices
***  DB persist for List Mode results for one or more defined analyzers
***  Rework List mode analyzer UI with tabbed dialog, WRT #69 Present the proposed List Mode Config UI ...
***  Scalar2 counts not summed when importing NCC files
11n  Measurement waiting for a neutron that never arrives (MCA-527)

6.0.1.15 Aug 26, 2016

126  Reanalyze
***  KVal selector for Euratom prepped for use; acquire mtl type case mismatch patch
***  Fix stratum list to all add
***  Create DB script includes JSR-15 entry
***  skip empty output line in report if no virtual SR results
***  Review dialogs measurement list sorting fixed
***  Fix Acquire from DB crash introduced in fb24f4fcb830bb0097a9ca319cfce708f5c0a7f5
78   Deming output to 'dmd' formatted file, Deming 'dmr' file view, input and content use
     using the 'Get curve-fitting results' import feature on the Curve Selection dlg
***  Show material only for ver,hld,cal measurements in lists
***  LM buffer extension detached circular buffer end


6.0.1.14 Aug 8, 2016

***	Fix LANL's Acquire Assay Cancel crash in 382f3ff7aa018b53580f11317bbd8370b6cdc949;
	Tool tip for Setup dialog detector type field for IAEA/Euratom;
	Update version and config branding with branch name;
	This is the first fork away from LANL's dev branch (aka 'master').

***	Calib curve dlg crash when material type name is not compared case-insensitively 
127	Pu mass limit in known alpha configuration did not allow negative numbers
118	Declared UMass was always overwriting declared Pu Mass for items
125	Known alpha acceptance limits added to measurement report
128	Mass limits for known alpha now persistent 

6.0.1.13 Aug 4, 2016

mmm	JSR15 HV setting was wrong. Would not work. Added log entry to screen for backup.


6.0.1.12 Aug 2, 2016
97	DB issues with non En language and region settings
32	Implement INCC5 File > Save As/Export > Transfer and Initial Data 
109	Item Id and Collar Item Id deletion
110	Collar Item Id needs Item Id defined at same time
75	LM wizard setting for cycle count not used in Live DAQ
76	LM wizard gatewidth param change not used in subsequent live DAQ analysis
73	Enhance predelay precision
43	Performing LM as SR after doing LM Acquire craps
113	Disk file inputs need the detector alpha-beta pre-filled


6.0.1.11 Jun 22, 2016
102	NCD file processing drops input
103	Transfer measurement selection dlg just like INCC5
104	HV plateau, waiting for a neutron that never arrives (MCA-527)

6.0.1.10 Jun 13, 2016
98	Acquisition measurement termination conditions
31	Implement the INCC5 Reanalysis
81	Tools for managing large sets of isotopics and items

6.0.1.8 May 27, 2016
85	Implement full SQL database implementational implementation
23	how to delete list mode detector? 
66	Delete measurements crashes the code

6.0.1.7 May 26, 2016
92	File locations are not fully coherent

6.0.1.6 May 16, 2016
46	HV Plateau enhancement
88	Show type (SR or LM) on facility setup dlg 
94	Add a comment column to the various measurement reports dialogues
93	File or folder query for list mode input files confusing
90	Some facility setup dlg buttons should be disbaled and/or removed

6.0.1.5 Apr 26, 2016
40	source ID in 'initial source' does not get passed to results
70	Add MCA-527 single channel list mode DAQ 
65	Isotopics should not be stored on cancel

6.0.1.4 Apr 9, 2016
67	PTR-32 data handling question - Joe, Heather

6.0.1.3 Apr 3, 2016
33	Implement INCC5 File > Get External Data > Stratum Authority and Item Relevant Data Files
11	Implement progress and cancel features for lengthy operations
80	Summary results CSV reports
82	Implement the report subsection filter feature

6.0.1.2 Mar 13, 2016
34	Implement Composite Isotopics dlg and use
83	Selecting isotopics for use

6.0.1.0 Feb 11, 2016
27	Cancel assay does not fully stop acquisition
38	Acquire Verification from file -- Cancel button pushed, INCC6 ignores
47	A second calib assay does not recall the previous calib assay item id
77	Add support for INCC5 date/time encoded file names
72	Enhance cmd line to support INCC5-style NCC processing 

6.0.0.1 Jan 27, 2016
68	Limit the instrument type selections for List Mode to those actually supported now
71	Add support for reading MCA-527 single channel list mode data files
54	Output location should be sub-specified into four optionally distinct paths
9	List mode analysis results reports can have empty sections        
58	When doing an LM measurement, start at Step 3 
50	Current acquire state retention and recall is not consistent
62	Save measurements only if successful or completed
74	Measurement report dialog print feature broken 
15	Implement Reports | Verification
51	LM input source file folder selection is too limited



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

PTR-32 support:

A USB device driver from Digilent is required for PTR-32 connectivity.
If you have installed the PTR-32 vendor's software, then the device driver is already present.
If not, install the PTR-32 vendors software, or obtain the drivers directly.

In the absence of the PTR-32 vendor's software, this package supports the PTR-32:
Digilent Adept 2.3, with DpcUtils 2.1

http://www.digilentinc.com/Data/Products/adept/DigilentAdept_v2-3-0.exe

Also available directly from the vendor, or elsewhere, is Diligent Adept 2.10.2, with DpcUtils 2.7.2.


Packages for older Windows systems (Vista, XP) are found at these links:

http://www.digilentinc.com/Products/Detail.cfm?NavPath=2,398,827&Prod=ADEPT

The installer DASV1-10-0.msi is for 32-bit OS.
http://www.digilentinc.com/Data/Products/ADEPT/DASV1-10-0.msi

The installer DASV1-10-0(x64).exe is for 64-bit OS.
http://www.digilentinc.com/Data/Products/ADEPT/DASV1-10-0(x64).exe


See www.digilent.com and PTR-32 documentation for additional details.
**********************************************************************************

