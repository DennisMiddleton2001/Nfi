=====================================================================================================================
NTFS VOLUME COMMANDS
=====================================================================================================================
EXAMPLES....  BTW, volme names should always be in the form:  "\\.\X:", and numbers are all hex.

	Nfi.exe -frs \\.\C: 0 1		- Dumps fie record 0 as parsed.
	Nfi.exe -frsh \\.\C: 0 1	- Dumps fie record 0 as hex.
	Nfi.exe -dir \\.\C: 5		- Dummps the root folder.
	Nfi.exe -attr \\.\C: 5 a0 $I30  - Dumps the root folder index allocation parsed.
	Nfi.exe -attrh \\.\C: 5 a0 $I30 - Dumps the root folder index allocation as hex.
	Nfi.exe -attrh \\.\C: 5 80 ""   - Dumps the $MFT as hex.
	Nfi.exe -attr \\.\C: 6 80 ""    - Dumps the volume bitmap parsed.
	Nfi.exe -attr \\.\C: 9 80 $SDS  - Dumps the security descriptors parsed.

Some of these commands take a LONG TIME!!!! If you need to break out with CTRL+C, you can.  The partial results will 
be in the current folder as ".\Tmp.txt".  For references to NTFS internals, refer to resources on the web.

=====================================================================================================================
NTFS USAGE REFERENCE
=====================================================================================================================
NFI -LBN [Volume] [Block] [NumberOfBlocks]                            :: Dumps logical block range.
NFI -LCN [Volume] [Block] [NumberOfBlocks]                            :: Dumps logical cluster range.

NFI -VOL [Volume] 			                              :: Dumps NTFS volume information.

NFI -FRS [Volume] [FileRecordNumber]	                              :: Dumps file record parsed.
NFI -FRSH [Volume] [FileRecordNumber]	                              :: Dumps file record as hex.

NFI -ATTR [Volume] [FileRecordNumber] [AttrTypeCode] [AttrName]	      :: Dumps attribute stream parsed.
NFI -ATTRH [Volume] [FileRecordNumber] [AttrTypeCode] [AttrName]      :: Dumps attribute stream as hex.
=====================================================================================================================

=====================================================================================================================
HDD USAGE REFERENCE
=====================================================================================================================
NFI -DISK				                              :: Dumps MBR/EFI partitioning info for harddisk.
NFI -DISK "-n"				                              :: Dumps MBR/EFI partitioning info no hex.

General Info
--------------
The -DISK option invokes SECINSPECT.EXE in the local folder.  You can actually do a lot more with it if you chain 
commands together (backup/restore sector ranges, etc.) Just make sure the SECINSPECT arguments are in quotes.  Read 
the Secinspect.CHM file for more details.

Example of chaining commands:

         Nfi.exe -DISK "-backup \\.\z: c:\backup.dsk 0 63"		- Backs up the NTFS boot area on Z:.
         Nfi.exe -DISK "-backup \\.\physicaldrive0: c:\backup.dsk 0 1"  - Backs up the MBR on disk 1.

=====================================================================================================================