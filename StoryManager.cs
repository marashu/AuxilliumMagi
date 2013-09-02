using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AuxilliumMagi
{
    class StoryManager
    {
        private static List<StoryElement> storyElement;
        public static List<StoryElement> Story{ get { return storyElement; } }

        public StoryManager()
        {
            storyElement = new List<StoryElement>();
        }

        public static void GenerateStory()
        {
            storyElement.Clear();
            //prologue     
            AddStoryElement(0, "Elder", "Welcome to the Academy training ground. I am", "here to supervise the Pairing between Ice Mage", "Zaganos and Fire Mage Aruna.");
            AddStoryElement(0, "Elder", "Fire and Ice are opposing forces, but united they", "can be stronger than any force in the world.");
            AddStoryElement(0, "Aruna", "Thank you for your assistance in this Pairing,", "Elder. I look forward to working with him.");
            AddStoryElement(0, "Zaganos", "I'm glad to have another partner to work with.", "Please proceed with the ceremony.");
            AddStoryElement(0, "Elder", "In order to form an effective Pairing, you will", "undergo a brief trial to show your talents to", "each other, and learn to co-operate.");
            AddStoryElement(0, "Aruna", "I'm ready, Elder.");
            AddStoryElement(0, "Zaganos", "I'm prepared as well.");
            AddStoryElement(0, "Elder", "Very well, Let the challenge commence! Use your", "powers to protect me from both fire and ice.");

            //level 1
            AddStoryElement(1, "Aruna", "I'm so excited. I've had this mission ready for a", "while now. I was just waiting for an Ice Mage to", "become available for Pairing to get rolling on it.");
            AddStoryElement(1, "Zaganos", "Well, I suddenly became available and you were", "recommended by the academy. You seem to be", "prepared, at least.");
            AddStoryElement(1, "Zaganos", "What exactly are we up to on this mission?");
            AddStoryElement(1, "Aruna", "There's a wealthy scholar in town who needs to", "be escorted to the worksite for a new Sky City", "that's being built.");
            AddStoryElement(1, "Zaganos", "Sky City? Those floating castle things? That's", "pretty impressive for a first mission. You're sure", "we can handle it?");
            AddStoryElement(1, "Aruna", "We're only doing some of the side work that", "they need done. I don't intend to get us involved", "in anything more troublesome unless");
            AddStoryElement(1, "Aruna", "you think we can handle it.");
            AddStoryElement(1, "Zaganos", "It's refreshing to be finally partnered up with a", "Fire Mage who has a sense of self-preservation.", "I think we'll get along well.");
            AddStoryElement(1, "Zaganos", "Now let's go find that scholar and get started!");

            //level 2
            AddStoryElement(2, "Zaganos", "This is heavy! I thought something called a", "Levistone would be lighter than this!");
            AddStoryElement(2, "Aruna", "That's a misconception. The Levistone actually", "stays on the ground, and objects placed above", "it can float.");
            AddStoryElement(2, "Zaganos", "This is preposterous. I'm a mage, not a mule!", "Why can't we at least have a cart or something?");
            AddStoryElement(2, "Aruna", "Think of this as your chance to handle a very", "expensive piece of magical equipment without", "having to pay for it."); 
            AddStoryElement(2, "Aruna", "And be part of a larger project which should", "last for centuries.");
            AddStoryElement(2, "Zaganos", "Agh. I'm picking the next mission. This sort", "of work is not what I was looking forward to.");
            AddStoryElement(2, "Zaganos", "I'm actually hoping bandits show up to steal", "it, just so I can put it down for a while.");
            AddStoryElement(2, "Aruna", "Well, those fellows back there have been", "following us for some distance.", "They might be up to no good.");
            AddStoryElement(2, "Zaganos", "Come on then! Come and get it!");

            //level 3
            AddStoryElement(3, "Zaganos", "Now this is more like it. No hauling rocks,", "no long walks down roads. Just a healing rune,", "and two chairs.");
            AddStoryElement(3, "Aruna", "This seems too easy for work.", "Isn't there any danger or excitement?");
            AddStoryElement(3, "Zaganos", "Nope. After the cleric inscribed the rune, he", "just needs it watched in case something happens.");
            AddStoryElement(3, "Zaganos", "In a few hours, the rune will have taken care", "of the plague here, and everyone can go back", "to their lives");
            AddStoryElement(3, "Aruna", "We're getting paid to sit here and wait for it", "to work and getting credit for saving a town?", "It almost seems dishonest.");
            AddStoryElement(3, "Zaganos", "Active runes have a habit of attracting magically", "active monsters. It's not entirely without risk.");
            AddStoryElement(3, "Zaganos", "There are also people who don't want a", "plague cured. Rival countries, swamp hags,", "necromancers; you never know.");
            AddStoryElement(3, "Aruna", "Necromancers? That sounds like a serious threat.");
            AddStoryElement(3, "Zaganos", "Yep. Necromancy's bad for magic. If a", "necromancer's spell hits you it can lock out", "control of your magic.");
            AddStoryElement(3, "Zaganos", "Still, real necromancers are pretty rare. I give it", "a less than one in a hundred shot of them being", "anywhere near here.");
            AddStoryElement(3, "Aruna", "One in a hundred, you say? Do they look anything", "like those cloaked figures over there?");
            AddStoryElement(3, "Zaganos", "What, over there? They can't be...");
            AddStoryElement(3, "Aruna", "Put your chair away, this is going to be", "an ugly fight.");

            //level 4
            AddStoryElement(4, "Aruna", "Do you think this will really drive the", "necromancers away?");
            AddStoryElement(4, "Zaganos", "It's certainly easier than hunting them through", "the swamps and getting them one by one.");
            AddStoryElement(4, "Aruna", "So instead, we're drawing them all to us?", "Sounds dangerous.");
            AddStoryElement(4, "Zaganos", "Only some of them. The rest will probably leave", "the area. It's going to be even worse than the", "rune incident, though.");
            AddStoryElement(4, "Aruna", "The Priestess with them kinda scared me. That", "god of hers seems powerful and all, but he", "also doesn't seem very nice.");
            AddStoryElement(4, "Zaganos", "She's even less nice than you think.", "She tried to sacrifice me once.");
            AddStoryElement(4, "Aruna", "What? That's crazy!");
            AddStoryElement(4, "Zaganos", "She's an old partner of mine. While working", "together she discovered the Sun god, and", "following him meant offering a sacrifice.");
            AddStoryElement(4, "Zaganos", "She assumed it meant me. She apologized later.", "Apparently giving up life as a Fire Mage", "was good enough.");
            AddStoryElement(4, "Aruna", "That still doesn't sound very well adjusted.");
            AddStoryElement(4, "Zaganos", "It was probably the worst break up of a Pairing", "I've ever heard of. Still, not as dangerous as", "some jobs out there.");
            AddStoryElement(4, "Aruna", "I guess.");
            AddStoryElement(4, "Aruna", "It seems like they've taken the bait.", "They're coming for us now; are you prepared?");

            //level 5
            AddStoryElement(5, "Zaganos", "I guess we can call that city safe now, though", "she's gotten a lot stronger from when I last", "worked with her.");
            AddStoryElement(5, "Aruna", "I'm not familiar with the magic she was using.", "What was that?");
            AddStoryElement(5, "Zaganos", "She's worshipping a Sun god, so I assume Sun", "magic? Maybe something divine?");
            AddStoryElement(5, "Zaganos", "And what is this place? Why are we in the middle", "of nowhere?");
            AddStoryElement(5, "Aruna", "This is the mountain that the Sky City is going", "to be built around. We're here to drive out", "the monsters.");
            AddStoryElement(5, "Aruna", "They've set up a nuisance rune to pester the", "monsters into leaving, but it's as likely to", "provoke an attack.");
            AddStoryElement(5, "Zaganos", "Evicting monsters from a mountain? Some of", "them have been here for hundreds of years and", "they know how to fight."); 
            AddStoryElement(5, "Zaganos", "I wouldn't expect them to leave if there's even", "a single arrow left to fire at us.");
            AddStoryElement(5, "Aruna", "There are plenty of mountains in the area. They", "can find a new home easy enough."); 
            AddStoryElement(5, "Aruna", "The Sky City's moving along pretty fast now.", "They could probably launch it soon enough.");
            AddStoryElement(5, "Zaganos", "So long as they don't ask me to drag any more", "stones around.");
            AddStoryElement(5, "Aruna", "Nope, just face the right direction and take out", "the arrows before they reach us.");

            //level 6
            AddStoryElement(6, "Zaganos", "Yep, plenty of mountains for them to settle in", "this area.");
            AddStoryElement(6, "Zaganos", "Too bad instead of settling, they called all the", "monsters in the neighboring mountains to help", "them kick us off their mountain!");
            AddStoryElement(6, "Aruna", "I can't believe we didn't see this coming.", "This looks pretty bad.");
            AddStoryElement(6, "Zaganos", "It gets worse. They've plunked down an altar", "here to pump some magic into the mountain", "to get the city ready to fly.");
            AddStoryElement(6, "Zaganos", "That sort of energy draws a lot of powerful", "creatures, like dragons.", "Do you want to fight a dragon?");
            AddStoryElement(6, "Aruna", "We can use the altar to support us, though. I", "think we can even borrow a little power from it", "to reflect attacks back at them.");
            AddStoryElement(6, "Aruna", "Once the altar's finished channeling into the", "mountain they should give it up. There's no way", "for them to stop the Sky City then.");
            AddStoryElement(6, "Zaganos", "I really am disliking this Sky City business.", "Nothing is ever easy with them.");
            AddStoryElement(6, "Zaganos", "Well, let's get ready. Here comes that dragon", "I was talking about.");

            //level 7
            AddStoryElement(7, "Zaganos", "Ah, easy times are here again.");
            AddStoryElement(7, "Aruna", "Are we just here to watch this druid", "smell flowers?");
            AddStoryElement(7, "Zaganos", "Absolutely not. He's investigating a serious", "taint that's sucking the magic out of", "the nearby lands.");
            AddStoryElement(7, "Zaganos", "It's still in the early stages. It's probably", "just that necromancer cult trying to set up a", "base again.");
            AddStoryElement(7, "Aruna", "So close to the town they just got tossed out of?", "That's crazy.");
            AddStoryElement(7, "Aruna", "And this bad energy. It doesn't feel like the", "necromancy we fought earlier. It's not paralyzing,", "it's more draining.");
            AddStoryElement(7, "Zaganos", "Whatever it is, it's best to try and avoid it.", "Those spells could really mess us up.");
            AddStoryElement(7, "Aruna", "That's what it feels like! It's a little bit like", "the energy from that altar the Sun Priestess set", "up.");
            AddStoryElement(7, "Aruna", "You know her best. Could this be her doing?");
            AddStoryElement(7, "Zaganos", "As I said, she tried to sacrifice me once. She's", "capable of anything. If it is her, she's got a", "whole town serving her now.");
            AddStoryElement(7, "Zaganos", "Stay prepared, Aruna, this just might be a", "whole lot more dangerous than I thought.");

            //level 8
            AddStoryElement(8, "Zaganos", "I can't believe her! This is mad even for her!", "She's turned the entire town into a cult.", "Madness!");
            AddStoryElement(8, "Aruna", "It's not safe to go back there.", "There are cultists swarming all over the", "countryside.");
            AddStoryElement(8, "Zaganos", "But she's up to something. We can't just let her", "sit there. Who knows what sort of damage she can", "do? We have to think of something.");
            AddStoryElement(8, "Aruna", "What about the Sky City? It's launching soon, and", "they'll need some protection for the Levistone", "as it takes off.");
            AddStoryElement(8, "Aruna", "They could do us a favor and drop us off in the", "town proper, and bypass the cultists", "in the countryside.");
            AddStoryElement(8, "Zaganos", "All we'd have to do then is make it to her altar", "and shut it down. This sounds like it could work.");
            AddStoryElement(8, "Aruna", "We've already hauled the Levistone, so we may", "as well be there when it gets used. It could also", "maybe help us use her power against her.");
            AddStoryElement(8, "Zaganos", "It's amazing how far the Sky City has come along.", "It feels nice to be a part of something so", "important. Now let's get this show on the road.");

            //level 9
            AddStoryElement(9, "Zaganos", "Well, descending from a Sky City is far from", "subtle. How many cultists do you figure saw us?");
            AddStoryElement(9, "Aruna", "Two, maybe three thousand? It's like we kicked an", "anthill out there. Once they organize, this", "altar's going to get swarmed.");
            AddStoryElement(9, "Zaganos", "I've managed to sabotage the altar. It's going to", "take time for the built-up power to dissipate.");
            AddStoryElement(9, "Aruna", "This is exciting. I always thought Paired mages", "were about performing contracts and guarding", "stuff."); 
            AddStoryElement(9, "Aruna", "I never would have guessed we'd end up here,", "in this sort of situation.");
            AddStoryElement(9, "Zaganos", "We've seen a lot in this short period of time.", "In case we don't make it out, you've been the", "best Fire Mage I've worked with by far.");
            AddStoryElement(9, "Aruna", "I think we can handle this. So long as they", "don't come all at once, we can push them back.");
            AddStoryElement(9, "Aruna", "The Sky City will come back to pick us up", "after the altar's down. We've just got to last", "long enough.");

            //level 10
            AddStoryElement(10, "Aruna", "Well done! The altar is broken.");
            AddStoryElement(10, "Zaganos", "Excellent. Now we've got to chase out the cult.", "Any ideas?");
            AddStoryElement(10, "Aruna", "We can threaten them. We've got a Sky City,", "and their source of magic has just been wrecked.");
            AddStoryElement(10, "Zaganos", "Let's do this.");
            AddStoryElement(10, "Zaganos", "ATTENTION SUN CULT! YOUR ALTAR", "HAS BEEN DESTROYED. LEAVE THIS", "TOWN IMMEDIATELY OR ELSE!");
            AddStoryElement(10, "Aruna", "OUR SKY CITY IS COMING FROM THE", "ACADEMY WITH A HUNDRED PAIRS!", "LOOK, IT APPROACHES!");
            AddStoryElement(10, "Aruna", "Wow, look at them run. I didn't expect", "it to work so well.");
            AddStoryElement(10, "Zaganos", "Nice bluff with the hundred Pairs.", "I wish I'd thought of that.");
            AddStoryElement(10, "Zaganos", "Now all we have to do is deal with the Sun", "Priestess.  She's bound to have an unpleasant", "bag of tricks handy.");
            AddStoryElement(10, "Aruna", "I'm as confident in my skills, and I'm confident", "in yours. We'll handle the surprises and scatter", "the rest of this cult in no time. Let's get 'em!");

            //level 11
            AddStoryElement(11, "Zaganos", "We've done it!  The Sun Priestess is defeated", "and the cult scattered.  What's next for us,", "Partner?");
            AddStoryElement(11, "Aruna", "I'm exhausted. Think we could take a short", "break and just relax on the Sky City for a while?");
            AddStoryElement(11, "Zaganos", "Naw, I know an easy job. You see, there's an", "alchemist who's working on a new antidote. I", "mean, who would want to stop an antidote?");
            AddStoryElement(11, "Aruna", "Sounds like it has potential to spiral", "wildly out of control. Why not?");
            AddStoryElement(11, "Zaganos", "That's the spirit!");
            

        }

        public static void ClearPreviousStoryElements()
        {
            for (int i = 0; i < storyElement.Count;)
            {
                if (storyElement[i].Level < ActionState.Level)
                    storyElement.Remove(storyElement[i]);
                else
                    return;
            }
        }

        public static void AddStoryElement(byte lvl, string speaker, string d1)
        {
            StoryElement tempStoryElement = new StoryElement(lvl, speaker, d1);
            storyElement.Add(tempStoryElement);
        }

        public static void AddStoryElement(byte lvl, string speaker, string d1, string d2)
        {
            StoryElement tempStoryElement = new StoryElement(lvl, speaker, d1, d2);
            storyElement.Add(tempStoryElement);
        }

        public static void AddStoryElement(byte lvl, string speaker, string d1, string d2, string d3)
        {
            StoryElement tempStoryElement = new StoryElement(lvl, speaker, d1, d2, d3);
            storyElement.Add(tempStoryElement);
        }
    }
}
