# Changelog
===

## V 0.9.0 Public

### Speed improvements
- All "heavy" loops are now parallel
- Disabled marks on plot lines for better performance

### Acoustic Data:
Now the app can show the acoustic test results for selected transducers.

**New tabs:**

- `FR` - Frequency Response
- `THD` - Total Harmonic Distortion
- `RNB` - Rub and Buzz
- `IMP` - Impedance
  
**All tabs consist of the following plots:**

- Acoustic test results for each die-side (4 plots)
  - `Grey` - acoustic test result for one transducer
  - `Red` - reference line
  - `Green` - Upper and Lower limit

**Mean Plots:**
4 mean lines for each die-side plotted on one plot for comparison (no limits for this plot)

**Acoustic Files**
Files with acoustic data by default are *found and selected automatically*:
- For each of selected process `.json` files an acoustic file with the same serial number is found automatically in `Z:` drive (acoustic files that have failed are skipped)
- If you don't have access to `Z:` drive you can select an option of `Manual Selection` for acoustic files

### Settings and Config

There are 2 new settings that you can change either in `Config/config.json` or in the application inside `Settings -> Acoustic` tab:
- `Show Acoustic Tabs` - you can show / not show acoustic tabs if you don'd need them
- `Manual Acoustic File Selection` - if you don't have access to `Z:` drive and have acoustic files on your computer, enable this option. Second file dialog will appear after you select process files where you can select acoustic files.
- **! NOTE !**: acoustic files **MUST** be as they are, in `.zip` format and with their original names.
- During manual selection all acoustic files will be checked the same way as in automatic selection - by serial number.

## New files and folders

In the application folder you will see new folders with files:
- `Config` - application configuration files
- `Limits` - limit files for acoustic tabs

**Limit files**
You can add up to 12 limit files in the folder `Limits`:
- Upper, Lower limits and Reference files (3 files)
- 3 files for 4 acoustic tabs

If you don't have part of limits or don't have them at all, they will not be displayed on the plots.
Also, limits should have the following names:
- `FRUpper.dB`, `IMPReference.Ohm`, etc
- `<Acoustic test name in short form><Type of limit>`
- Extension of the file does not matter, it should be just a text file with no text header (only values in the file)
- So, if you want to add limits to `Frequency Response` you should add the following files to the folder:
  - `FRUpper.<>`, `FRLower.<>`, `FRReference.<>`
  - Names of the files are strict and if named in an incorrect way, will not be shown

## Samples

In the application folder you will find sample files with limits and process/acoustic data for testing.
===

## V 0.7.1 Beta
- Backend changes: 
  - More generic and flexible interface for extendebility
- UI changes:
  - Now plot area has a crosshair with precise coordinates and follows the mouse cursor
  - You can use it to check the values of the plot more precisely
  - To upload new files just click again on **Select Files(s)** and after selecting new files the data on the screen will be updated
  - Shows the amount of files for each DieSide
  - Plots *all* curves that were inputed in FileDialog
  - Calculates and shows *mean* features for each DieSide
- Further work:
  - Some errors and bugs are present:
    - When cancel file selection, throws an error
    - May be incompatibe with some JSON files that are missing ps01_heater_on or other steps
  - Increase the speed of processing data
===
