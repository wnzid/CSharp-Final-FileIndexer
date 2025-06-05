# File Indexer

A multi-process C# project for distributed `.txt` file indexing using named pipes, multithreading, and CPU core affinity.

---

## Overview

This system consists of three separate C# console applications:
- ScannerA
- ScannerB
- Master

Each scanner reads `.txt` files from a selected directory, counts word frequencies, and sends the results to the master. The master aggregates and displays the final combined result.

---

## How to Run

1. Compile the Projects  
   Build all three console applications: `ScannerA`, `ScannerB`, and `Master`.

2. Start the Master  
   Run the Master console app first. It will wait for connections from the scanners:  

3. Start ScannerA and ScannerB (separately)  
   Launch each scanner in a new terminal window. Input the folder path containing `.txt` files:  

4. Follow the Prompts  
   - Enter folder paths when asked  
   - Confirm whether to send results to the master  
   - The master will output the combined word counts from both agents

---

## Technologies Used

- Named pipes (NamedPipeServerStream, NamedPipeClientStream)
- Multithreading (Thread)
- CPU core binding (ProcessorAffinity)
- Console Applications

---

## Version History

v1.0 - initial scannerA <br/>
v1.1 - ask for folder path and check if it exists or not <br/>
v1.2 - list all .txt files in the selected folder <br/>
v1.3 - read first .txt file line by line and print it <br/>
v1.4 - read all .txt files in the selected folder and combine them <br/>
v1.5 - count word frequency in each .txt file and store in dictionary <br/>
v1.6 - format output as filename:word:count per line <br/>
v1.7 - added loop to let user choose another folder <br/>
v1.8 - skip empty files <br/>
v2.0 - added pipe server waiting on 'agent1' <br/>
v2.1 – ScannerA connects to pipe and sends a test message <br/>
v2.2 - ScannerA sends file content to pipe server(master) <br/>
v2.3 - Asking user if they want to send data to master <br/>
v2.4 - input validation in scannerA <br/>
v3.0 - added ScannerB (clonned scannerA) <br/>
v3.1 – Master handles ScannerA and ScannerB sequentially <br/>
v3.2 - Master gives combined result of both scanners <br/>
v4.0 - added multithreading to ScannerA (read and send run in separate threads) <br/>
v4.1 - added multithreading to ScannerB (read and send run in separate threads) <br/>
v4.2 - added multithreading to Master (handle ScannerA and ScannerB in separate threads) <br/>
v4.3 – set ProcessorAffinity in each <br/> 

---
@wnizd

