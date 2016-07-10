**********************************************************************************

INCC6 Beta Release 12
6.0.1.12 July 10, 2016 J. Longo

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
   Holdup and glovebox features, the Plot features,
   Deming integration, some Add-a-src processing, Collar item data entry,
   Collar acquire and Poison Rod details, integrated help.

This is beta release software; issues and missing features are known to exist.

CONTACT:

Please use direct email to j.longo@iaea.org or heather@lanl.gov for support and issue reporting.

ISSUES
See https://github.com/hnordquist/INCC6/issues


Issues for basic INCC5/6 feature COMPLETION

#84*	Implement Collar
105*	Measurement, waiting for a neutron that never arrives (MCA-527)
#101+   Coincidence matrix calculation needs completion
#99+    Time interval summary incomplete
#79+	Plot Norm, Plot Ver diagnostic tools
#76*	LM wizard gatewidth param change not used in subsequent live DAQ analysis
#75*	LM wizard setting for cycle count not used in Live DAQ

#78+	Find Deming curve fitting tool replacement
#69+	Present the proposed List Mode Config UI from DB enhancement
#35+	Hold-up analysis required
#32+    Implement INCC5 File > Save As/Export > Transfer and Initial Data


*HIGH Priority, +MEDIUM Priority

#100 Rossi processing too slow and can sometimes hang
#67 PTR-32 data handling question 
#59 Toggle for Fast/Conventional on the Meas Params dlg for LM not working 
#45 calc_alpha_beta performance and efficiency
#43 Performing LM as SR after doing LM Acquire craps*
#25 List mode acquisition predelay not stored
#16 Accidental Singles test Failure for PTR32
#13 PTR-32 -- Results different if reading file/doing live acquisition

Closed issues

 104 HV plateau, waiting for a neutron that never arrives (MCA-527)
 103 Transfer measurement selection dlg just like INCC5
 102 NCD file processing drops input
  98 Acquisition measurement termination conditions on dlgs NYI
  97 DB issues with non En language and region settings
  94 Add a comment column to the various measurement reports dialogues 
  93 File or folder query for list mode input files confusing 
  92 File locations are not fully coherent 
  88 Show type (SR or LM) on facility setup dlg 
  85 Implement full SQL database implementational implementation 
  83 Selecting isotopics for use 
  82 Implement the report subsection filter feature
  81 Tools for managing large sets of isotopics and items
  80 Summary results CSV reports 
  77 Add support for INCC5 date/time encoded file names 
  74 Measurement report dialog print feature broken 
  72 Enhance cmd line to support INCC5-style NCC processing 
  71 Add support for reading MCA-527 single channel list mode data files 
  70 Add MCA-527 single channel list mode DAQ 
  68 Limit the instrument type selections for List Mode to those actually supported now 
  67 PTR-32 data handling question - Joe, Heather 
  66 Delete measurements crashes the code 
  65 Isotopics should not be stored on cancel 
  62 Save measurements only if successful or completed 
  58 When doing an LM measurement, start at Step 3 
  54 Output location should be sub-specified into four optionally distinct paths 
  51 LM input source file folder selection is too limited
  50 Current acquire state retention and recall is not consistent 
  47 A second calib assay does not recall the previous calib assay item id
  46 HV Plateau enhancement 
  40 source ID in 'initial source' does not get passed to results
  38 Acquire Verification from file -- Cancel button pushed, INCC6 ignores 
  34 Implement Composite Isotopics dlg and use
  33 Implement INCC5 File > Get External Data > Stratum Authority and Item Relevant Data Files
  31 Implement the INCC5 Reanalysis
  27 Cancel assay does not fully stop acquisition
  23 how to delete list mode detector? 
  15 Implement Reports | Verification 
  11 Implement progress and cancel features for lengthy operations
   9 List mode analysis results reports can have empty sections 


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

