World of Tanks Match Making Simulator
====================================

##What is this and why do I care?##

This is a test harness for developing newer, better matchmaking algorithms. It features a basic interpretation of the wargaming guidlines on the current matchmaking as a reference implementation.

The reference implementation was mostly built from [this page](http://wiki.wargaming.net/en/Battle_Mechanics#Battle_Tiers).

##I want to implement an algorithm, how do I get started##

Fork the repository, make a new class that inherits `MatchmakerBase` and go from there. The only method you have to implement is `TryFormMatch`.


