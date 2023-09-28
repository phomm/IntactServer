USE [Intact_]
GO
/*
truncate table [dbo].[Factions]
truncate table [dbo].[ProtoWarriors]
truncate table [dbo].[ProtoBuildings]
truncate table [dbo].[Localizations]
*/
INSERT INTO [dbo].[Factions] ([Id] ,[Number]) VALUES
('Humans' ,1),
('Elves' ,2),
('Necros' ,3),
('Mages' ,4),
('Orcs' ,5)

INSERT INTO [dbo].[ProtoWarriors] ([Id] ,[Number] ,[FactionId] ,[Force] ,[AssetId],[IsHero],[IsRanged],[IsMelee],[IsBlockFree],[IsImmune],[InLife],[InMana],[InMoves],[InActs],[InShots],[Cost]) VALUES 
('Pikeman',1,'Humans',/*force*/1,'PikemanAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,1/*Life*/,0/*Mana*/,1/*Moves*/,1/*Acts*/,0/*Shots*/,2/*Cost*/)   ,
('Archer',2,'Humans',/*force*/2,'ArcherAsset',0/*Hero*/,1/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,1/*Life*/,0/*Mana*/,1/*Moves*/,1/*Acts*/,2/*Shots*/,3/*Cost*/)    ,
('Eagle',3,'Humans',/*force*/3,'EagleAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,1/*BlockFree*/,0/*Immune*/,1/*Life*/,0/*Mana*/,4/*Moves*/,1/*Acts*/,0/*Shots*/,4/*Cost*/)     ,
('Horseman',4,'Humans',/*force*/4,'HorsemanAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,1/*BlockFree*/,0/*Immune*/,2/*Life*/,0/*Mana*/,3/*Moves*/,1/*Acts*/,0/*Shots*/,5/*Cost*/)  ,
('Elephant',5,'Humans',/*force*/5,'ElephantAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,4/*Life*/,0/*Mana*/,1/*Moves*/,1/*Acts*/,0/*Shots*/,6/*Cost*/)  ,
('Knight',6,'Humans',/*force*/4,'KnightAsset',1/*Hero*/,1/*Ranged*/,1/*Melee*/,1/*BlockFree*/,0/*Immune*/,2/*Life*/,3/*Mana*/,3/*Moves*/,1/*Acts*/,2/*Shots*/,0/*Cost*/)    ,

('Scout',7,'Elves',/*force*/1,'ScoutAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,1/*Life*/,0/*Mana*/,1/*Moves*/,2/*Acts*/,0/*Shots*/,2/*Cost*/)      ,
('Sniper',8,'Elves',/*force*/2,'SniperAsset',0/*Hero*/,1/*Ranged*/,0/*Melee*/,0/*BlockFree*/,0/*Immune*/,1/*Life*/,0/*Mana*/,1/*Moves*/,1/*Acts*/,3/*Shots*/,3/*Cost*/)     ,
('Dryad',9,'Elves',/*force*/3,'DryadAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,1/*Life*/,0/*Mana*/,1/*Moves*/,1/*Acts*/,0/*Shots*/,4/*Cost*/)      ,
('Unicorn',10,'Elves',/*force*/4,'UnicornAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,1/*BlockFree*/,0/*Immune*/,2/*Life*/,0/*Mana*/,3/*Moves*/,1/*Acts*/,0/*Shots*/,5/*Cost*/)   ,
('Ent',11,'Elves',/*force*/5,'EntAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,4/*Life*/,2/*Mana*/,1/*Moves*/,1/*Acts*/,0/*Shots*/,6/*Cost*/)       ,
('Druid',12,'Elves',/*force*/2,'DruidAsset',1/*Hero*/,1/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,2/*Life*/,3/*Mana*/,1/*Moves*/,1/*Acts*/,3/*Shots*/,0/*Cost*/)     ,

('Ghoul',13,'Necros',/*force*/1,'GhoulAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,1/*Life*/,0/*Mana*/,1/*Moves*/,1/*Acts*/,0/*Shots*/,2/*Cost*/)    ,
('Lich',14,'Necros',/*force*/2,'LichAsset',0/*Hero*/,1/*Ranged*/,0/*Melee*/,0/*BlockFree*/,1/*Immune*/,1/*Life*/,2/*Mana*/,1/*Moves*/,1/*Acts*/,2/*Shots*/,3/*Cost*/)     ,
('Ghost',15,'Necros',/*force*/3,'GhostAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,1/*Life*/,0/*Mana*/,4/*Moves*/,1/*Acts*/,0/*Shots*/,4/*Cost*/)    ,
('Nightmare',16,'Necros',/*force*/4,'NightmareAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,1/*BlockFree*/,0/*Immune*/,2/*Life*/,3/*Mana*/,3/*Moves*/,1/*Acts*/,0/*Shots*/,5/*Cost*/),
('Mutant',17,'Necros',/*force*/5,'MutantAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,4/*Life*/,0/*Mana*/,1/*Moves*/,1/*Acts*/,0/*Shots*/,6/*Cost*/)   ,
('Vampire',18,'Necros',/*force*/1,'VampireAsset',1/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,2/*Life*/,3/*Mana*/,2/*Moves*/,1/*Acts*/,0/*Shots*/,0/*Cost*/)  ,

('Golem',19,'Mages',/*force*/1,'GolemAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,1/*Immune*/,1/*Life*/,0/*Mana*/,1/*Moves*/,1/*Acts*/,0/*Shots*/,2/*Cost*/)     ,
('Mage',20,'Mages',/*force*/2,'MageAsset',0/*Hero*/,1/*Ranged*/,0/*Melee*/,0/*BlockFree*/,0/*Immune*/,1/*Life*/,2/*Mana*/,1/*Moves*/,1/*Acts*/,2/*Shots*/,3/*Cost*/)      ,
('Gargoyle',21,'Mages',/*force*/3,'GargoyleAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,1/*Life*/,0/*Mana*/,4/*Moves*/,1/*Acts*/,0/*Shots*/,4/*Cost*/)  ,
('Lion',22,'Mages',/*force*/4,'LionAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,1/*BlockFree*/,0/*Immune*/,2/*Life*/,0/*Mana*/,3/*Moves*/,1/*Acts*/,0/*Shots*/,5/*Cost*/)      ,
('Giant',23,'Mages',/*force*/5,'GiantAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,4/*Life*/,2/*Mana*/,1/*Moves*/,1/*Acts*/,0/*Shots*/,6/*Cost*/)     ,
('Genie',24,'Mages',/*force*/3,'GenieAsset',1/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,1/*Immune*/,2/*Life*/,4/*Mana*/,1/*Moves*/,1/*Acts*/,0/*Shots*/,0/*Cost*/)     ,

('Goblin',25,'Orcs',/*force*/1,'GoblinAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,1/*Life*/,0/*Mana*/,1/*Moves*/,1/*Acts*/,0/*Shots*/,2/*Cost*/)     ,
('Orc',26,'Orcs',/*force*/2,'OrcAsset',0/*Hero*/,1/*Ranged*/,0/*Melee*/,0/*BlockFree*/,0/*Immune*/,1/*Life*/,0/*Mana*/,1/*Moves*/,1/*Acts*/,2/*Shots*/,3/*Cost*/)        ,
('Wyvern',27,'Orcs',/*force*/3,'WyvernAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,1/*Life*/,0/*Mana*/,4/*Moves*/,1/*Acts*/,0/*Shots*/,4/*Cost*/)     ,
('Rider',28,'Orcs',/*force*/4,'RiderAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,1/*BlockFree*/,0/*Immune*/,2/*Life*/,0/*Mana*/,3/*Moves*/,1/*Acts*/,0/*Shots*/,5/*Cost*/)      ,
('Ogre',29,'Orcs',/*force*/5,'OgreAsset',0/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,4/*Life*/,0/*Mana*/,1/*Moves*/,1/*Acts*/,0/*Shots*/,6/*Cost*/)       ,
('Cyclop',30,'Orcs',/*force*/2,'CyclopAsset',1/*Hero*/,0/*Ranged*/,1/*Melee*/,0/*BlockFree*/,0/*Immune*/,1/*Life*/,0/*Mana*/,1/*Moves*/,1/*Acts*/,0/*Shots*/,0/*Cost*/)

INSERT INTO [dbo].[Localizations] ([TermId] ,[LanguageCode] ,[Value]) VALUES
('HumansFactionName','English','Humans'),
('HumansFactionDescription','English','Humans have strong cavalry and brave disciplined army, loyal to their mighty hero-Knight with tactical skills and holy powers'),
('ElvesFactionName','English','Elves'),
('ElvesFactionDescription','English','Elves have strong archers and enchanted creatures of the forrest, hero-Druid helps them to reveal Nature powers'),
('NecrosFactionName','English','Necros'),
('NecrosFactionDescription','English','Necros are magically stabilized organisms, that could be revived by hero-Vampire, with strong infantry and frightening monsters as allies'),
('MagesFactionName','English','Mages'),
('MagesFactionDescription','English','Mages are powered with magic and antimagic abilities, they have elemental and mechanical creatures and hero - powerful Genie'),
('OrcsFactionName','English','Orcs'),
('OrcsFactionDescription','English','Warriors of the steppes and deserts are distinguished by endurance and mobility and the absolute non-recognition of higher magic'),

('PikemanWarriorName','English','Pikeman'),
('PikemanWarriorDescription','English','Pikeman is good infantry, always ready to fight bravely'),
('ArcherWarriorName','English','Archer'),
('ArcherWarriorDescription','English','Archer is good shooter with high discipline'),
('EagleWarriorName','English','Eagle'),
('EagleWarriorDescription','English','Eagle is fast but weak predator bird'),
('HorsemanWarriorName','English','Horseman'),
('HorsemanWarriorDescription','English','Horseman is strong cavalry, with jousting he fights fast and effective'),
('ElephantWarriorName','English','Elephant'),
('ElephantWarriorDescription','Elephant','Elefant is wellknown for its strength and toughness, it is able to crush fortifications, its natural rage makes him fearless and unstopable'),
('KnightWarriorName','English','Knight'),
('KnightWarriorDescription','English','Knight is a horseman with crossbow and spear, has jousting, casts holy light spell that stuns enemies, and heals his allies'),


('ScoutWarriorName','English','Scout'),
('ScoutWarriorDescription','English','Elf scout is fighter with two blades, he is well trained to strike two targets and has great agility to avoid jousting'),
('SniperWarriorName','English','Sniper'),
('SniperWarriorDescription','English','Elf sniper is a master-archer, able to shot more often than average shooter'),
('DryadWarriorName','English','Dryad'),
('DryadWarriorDescription','English','Dryad is a forrest mystical creature, it can absorb hostile magic, extract mana from it and then pass it to Druid'),
('UnicornWarriorName','English','Unicorn'),
('UnicornWarriorDescription','English','Unicorn is Elven cavalry that fights fiercely and protects allies, it provides allies with antimagic to hostile spells'),
('EntWarriorName','English','Ent'),
('EntWarriorDescription','English','Ent is unique creature of Nature, ancient nation of semitrees, their powerful Roots are able to ruin objects or entangle enemies'),
('DruidWarriorName','English','Druid'),
('DruidWarriorDescription','English','Druid is a well-trained Elven prince with ranged and melee weapons, with his extraagility he dodges shots or jousting strikes, calls to Nature to grow ''Treewall'' and ''Roots'' and able to shoot ''Chanted arrow'''),

('GhoulWarriorName','English','Ghoul'),
('GhoulWarriorDescription','English','Ghoul is raised undead, projectiles have no effect piercing its flesh, they do not feel pain'),
('LichWarriorName','English','Lich'),
('LichWarriorDescription','English','Lich is sorcerer who cheated death using strong magic, that also counters enemy magic, he casts ''Weakness'', that slows enemies'),
('GhostWarriorName','English','Ghost'),
('GhostWarriorDescription','English','Ghost is half-material spirit of dead, it dissolves through objects, but affected to weapon and magic, those free the spirit, and it can not be returned'),
('NightmareWarriorName','English','Nightmare'),
('NightmareWarriorDescription','English','Nightmare is fearsome halfdead werewolf beast, it can fears enemies with its outofthegrave scream'),
('MutantWarriorName','English','Mutant'),
('MutantWarriorDescription','English','Mutant is huge agglomerate of undead flesh, that smashes everything nearby, gathers and swallows dead bodies healing himself'),
('VampireWarriorName','English','Vampire'),
('VampireWarriorDescription','English','Vampire exchanged poor existence to Eternal Life, granted by constant draining others live powers. He paralyses enemies with hypnotic Fear and can rise Undead, for they to serve Death'),

('GolemWarriorName','English','Golem'),
('GolemWarriorDescription','English','Golem is magically animated fighting mechanism, that magic counters any other magic aimed on Golem, dismatled Golem looses all magic and becomes useless'),
('MageWarriorName','English','Mage'),
('MageWarriorDescription','English','Mage is a sorcerer, who spent all his life to study Magic. In battle he uses magiс to fire missiles into enemies and also protects allied warriors from enemy shots'),
('GargoyleWarriorName','English','Gargoyle'),
('GargoyleWarriorDescription','English','Gargoyle is a magical composition of elemental spirit and a stoneshell. The connection is quite thin and magic or shot rips it. Fortunately Giants can inspire the spiritless body if it was not broken'),
('LionWarriorName','English','Lion'),
('LionWarriorDescription','English','Lion is a King of animals, trained by mages to execute their orders. He rushes towards enemies and jumps over their heads to make furious attacks in backlines, thus making enemies feel uncomfortable with Lions'),
('GiantWarriorName','English','Giant'),
('GiantWarriorDescription','English','Giant is of the same age as Gods, whose life goes to Eternity. His elemental spirit is not bounded to body and can travel alone, in his powers - the inspiration of defeated Gargoyle, sharing part of spirit with them and bringing the control of Gargoyle back'),
('GenieWarriorName','English','Genie'),
('GenieWarriorDescription','English','Genie is the Master of Magic. He causes destructible Earthquakes and fires a ray of pure magic into enemies. With his mercy he grants his allies with haste and shares with them his antimagic protection for a while'),

('GoblinWarriorName','English','Goblin'),
('GoblinWarriorDescription','English','Goblins are good'),
('OrcWarriorName','English','Orc'),
('OrcWarriorDescription','English','Orcs are good'),
('WyvernWarriorName','English','Wyvern'),
('WyvernWarriorDescription','English','Wyverns are good'),
('RaiderWarriorName','English','Raider'),
('RaiderWarriorDescription','English','Raiders are good'),
('OgreWarriorName','English','Ogre'),
('OgreWarriorDescription','English','Ogres are good'),
('CyclopWarriorName','English','Cyclop'),
('CyclopWarriorDescription','English','Cyclop is Orcish hero')