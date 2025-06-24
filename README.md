# Documentation for the `avalonia` Repository

## 1. Overall Repository Overview

This repository is a collection of projects developed using the cross-platform UI framework **Avalonia** with C#. It serves as a practical demonstration of creating a complete application as well as learning specific aspects of the framework through a series of hands-on labs.

The repository is divided into two main parts:

1.  **`Reversi`**: A finished project—the "Reversi" board game with a graphical user interface and artificial intelligence.
2.  **`AvaloniaAplication`**: A set of laboratory works, each focusing on a specific feature or concept within Avalonia.

---

## 2. Part I: Reversi Project

This is a fully functional implementation of the classic board game "Reversi" (also known as Othello). The project is an excellent example of building a ready-to-use desktop application on Avalonia using the MVVM architectural pattern.

### Reversi Key Features:

* **Gameplay:** Fully implemented game rules on an 8x8 board with highlighting for valid moves.
* **Game Modes:** Player vs. Player (PvP) and Player vs. Bot (PvE).
* **Multiple Bot Types:**
    * **Greedy:** Chooses the move that captures the most pieces.
    * **Positional:** Evaluates the strategic value of cells.
    * **MiniMax:** Calculates moves ahead to select the optimal solution.
* **Customization:** Ability to switch between light and dark UI themes.
* **Technologies:** C#, .NET, Avalonia, MVVM (ReactiveUI).

*(The full documentation for this project in Reversi directory).*

---

## 3. Part II: Laboratory Works (`AvaloniaAplication`)

This directory contains a series of small, isolated projects, each representing a "laboratory work." The goal of these labs is to incrementally learn and demonstrate the key features of the Avalonia framework.

### Structure of the Laboratory Works:

* **Lab 1: Avalonia Basics**
    * **Objective:** To get acquainted with the basic structure of an Avalonia project.
    * **Demonstrates:** Creating a simple window, placing basic controls: `TextBlock`, `Button`, and reactive function of changing by button background color.

* **Lab 2: World Clock**
    * **Objective:** To learn how to colobarate REST API with UI.
    * **Demonstrates:** The first application, as I understand, is a World Clock built with Avalonia. A very useful utility! It's great that you can select a city from the list and immediately see the local time. The interface is clean and straightforward.

* **Lab 3: Memory Game**
    * **Objective:** To manage multiple windows within an application.
    * **Demonstrates:** The application is the classic and beloved game for memory training. The nice dark design, clear attempt counter, and 4x4 card grid are everything needed for an engaging game.
