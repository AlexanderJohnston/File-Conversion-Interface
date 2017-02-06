# Note to the reader

A screenshot of the GUI is available as "Interface.png" in the root. Personal information of customers has been blacked out.

I am aware of the lack of inheritance using classes and some betrayal of SOLID principles in this repo. I rushed the first codebase and am in the process of refactoring it.

**Please navigate to** /DataConversionInterface/Conversion Interface/MainForm.cs to begin reading.

# File-Conversion-Interface

This is a new tool for interacting with all of our back end systems. This is a re-design of the Powershell conversion interface, which was my first project.

# Synopsis

Now that our entire codebase has been refactored into a modern OOP language, we need a new front end interface. This is the beginning of my project to build one. It is my hope that this will open up our tools to the whole company, rather than just 2 programmers. Everything will need to be robust, simplified, and with lots of error handling.

This is an official project for work and the start date is 12/13/2016. Expected time to finish: 2-4 weeks, pending analysis.

Goals:
  1. Allow all users to have secure logins with hashed and salted passwords.
  2. Allow users to interact with a global file queue so that they do not overlap with each others' work.
  3. Interface should be able to report current step in back-end scripts, as well as reset/pause/stop them.
  4. Interface should be able to recognize when new data files are downloaded and categorize them automatically.
  5. Simplify, simplify, simplify. Currently, users need some technical skills to be able to use the back-end scripts. This interface should require no prior computer skills above "level 1" to use.

Thank you,

Alexander Johnston
