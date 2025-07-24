=== Witch

=Witch_1
Player: Who… are you? #avatar:player_01
Witch: Who I am doesn’t matter. What I know does. #avatar:witch

* [What do you know?] -> Witch_1_know
* [What’s wrong with the orchard?] -> Witch_1_orchard


= Witch_1_know
Witch: Something’s wrong in the forest. Something is rotting. Growing… wrong. #avatar:witch
* [What do you mean, growing wrong?]
    Witch: The mushrooms. They’re back. Too red. Too alive. #avatar:witch
    -> Witch_1_quest
* [Why is that bad?]
    Witch: Because it spreads. And once it does, no one is safe. #avatar:witch
    -> Witch_1_quest

= Witch_1_orchard
Witch: It’s not just the orchard. It’s the whole forest. The rot. It’s starting again. #avatar:witch
Witch: The mushrooms. They’re back. Too red. Too alive. #avatar:witch
-> Witch_1_quest


= Witch_1_quest
Witch: Six mushrooms. That’s all I need… for now. #avatar:witch
Witch: But listen carefully. Don’t eat them. Don’t even smell them. #avatar:witch
Witch: And if one of them speaks to you… don’t answer. Ever. #avatar:witch

* [What happens if I do?]
    Witch: Then you’ll owe me another favor. A much harder one. And I’d rather save that… for later. #avatar:witch
    -> END
* [Why would a mushroom speak?]
    Witch: Some of them… aren’t just mushrooms anymore. They’ve been here too long. #avatar:witch
   -> END


=Witch_2 
Witch: Have you found the mushrooms yet? Or are you just here to chat? #avatar:witch

+ [Not yet, but I have some questions.] -> Witch_2_questions
+ [They… talked to me.] -> Witch_2_talked

= Witch_2_questions
Witch: Make it quick. The forest is not getting any safer. #avatar:witch
+ [Why six mushrooms?]
    Witch: Because six is just enough to stop what’s starting… I hope. #avatar:witch
    -> END
+ [What if I find more?]
    Witch: Bring them if you like. I may find use for them later. #avatar:witch
    -> END

= Witch_2_talked
Witch: I told you not to listen! Did you answer them? #avatar:witch
+ [No, I stayed quiet.]
    Witch: Good. That makes it easier for both of us. #avatar:witch
    -> END
+ [Maybe. What happens now?]
    Witch: Then you’ll have to take care of that mistake… when the time comes. #avatar:witch
    -> END


=Witch_3
Witch: Ah… thank you. You brought them. #avatar:witch

Witch: …Oh no. It’s even worse than I feared. #avatar:witch

* [What do you mean?]
    Witch: The rot… it’s already spread deeper. This is just the beginning. #avatar:witch
    -> Witch_3_end
* [So… what now?]
    Witch: If you’re ready, I have something else you can do. Something… bigger. #avatar:witch
    -> Witch_3_end


= Witch_3_end
Witch: Are you ready for your next adventure? #avatar:witch#avatar:witch#avatar:witch#avatar:witch
-> END

=Witch_4
Player: What the hell happened?!#avatar:player_01
Witch: Oh no.... it already begun..#avatar:witch#avatar:witch#avatar:witch#avatar:witch
Player: What???#avatar:player_01
Witch: Everything changed...I feel it...#avatar:witch#avatar:witch#avatar:witch
Witch: You need to find my friend Emeritus as fast as possible.#avatar:witch#avatar:witch 
Witch: He is the only one who can stop it!#avatar:witch
->END