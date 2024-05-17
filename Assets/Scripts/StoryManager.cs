using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class StoryManager: MonoBehaviour
{
    public int storyNumber = 10;
    public string[] articles = {
        "Graphic design involves creating visual content to communicate messages. It combines typography, imagery, and layout techniques to produce advertisements, websites, logos, and more. This field has evolved with technology, incorporating digital tools and techniques to enhance creativity and efficiency.",
        "Origami, the Japanese art of paper folding, transforms a flat sheet of paper into a finished sculpture through folding techniques. Traditional designs include cranes and flowers, while modern origami can be complex, involving mathematical principles and intricate patterns.",
        "Video games have evolved from simple pixelated graphics to immersive, lifelike experiences. The industry began in the 1970s with arcade games like Pong and has grown into a multi-billion-dollar industry, encompassing a wide range of genres and platforms.",
        "The internet, a global network of computers, was developed in the late 20th century. It revolutionized communication, commerce, and information access. Initially a project by the U.S. Department of Defense, it has since become an indispensable part of modern life.",
        "Ludwig van Beethoven's Ninth Symphony, completed in 1824, is one of his most famous works. It includes the 'Ode to Joy,' a choral finale that has become a symbol of hope and unity. Beethoven was completely deaf when he composed this masterpiece.",
        "Albert Einstein's Theory of Relativity, formulated in the early 20th century, revolutionized our understanding of space, time, and gravity. It consists of the Special and General Relativity theories, explaining how objects move at high speeds and how gravity affects space-time.",
        "Black holes are regions in space where gravity is so strong that nothing, not even light, can escape. They form when massive stars collapse under their own gravity at the end of their life cycles. Black holes are studied to understand more about the nature of the universe.",
        "The Gutenberg Press, invented by Johannes Gutenberg in the mid-15th century, revolutionized the production of books. It allowed for mass production, making literature and information more accessible and affordable, and spurred the spread of knowledge during the Renaissance.",
        "The Renaissance, spanning the 14th to the 17th century, was a period of renewed interest in classical art and humanism. Artists like Leonardo da Vinci and Michelangelo produced masterpieces that emphasized realism, perspective, and the human form, profoundly impacting Western art.",
        "Tennis evolved from a game played by French monks in the 12th century to the modern sport we know today. The open tournaments, Grand Slam events, and influential players like Serena Williams and Roger Federer have popularized tennis globally."
    };

    public string[] LCLQuestion = {
        "What is graphic design primarily concerned with?",
        "What is origami?",
        "When did the video game industry begin?",
        "What was the internet initially developed by?",
        "What is included in Beethoven's Ninth Symphony?",
        "Who formulated the Theory of Relativity?",
        "What can escape from a black hole?",
        "Who invented the Gutenberg Press?",
        "Who was a famous Renaissance artist?",
        "Where did tennis originally evolve from?"
    };

    public string[] LCLChoice1 = {
        "Creating visual content to communicate messages",
        "The art of paper folding",
        "1970s",
        "U.S. Department of Defense",
        "The Four Seasons",
        "Isaac Newton",
        "Nothing",
        "Johannes Gutenberg",
        "Pablo Picasso",
        "English knights"
    };

    public string[] LCLChoice2 = {
        "Writing novels",
        "The art of painting",
        "1990s",
        "U.S. Department of Commerce",
        "Ode to Joy",
        "Albert Einstein",
        "Light",
        "Leonardo da Vinci",
        "Leonardo da Vinci",
        "French monks"
    };

    public string[] HCLQuestion = {
        "How has technology impacted graphic design?",
        "How has modern origami evolved from traditional designs?",
        "How have video games evolved since their inception?",
        "How has the internet impacted modern life?",
        "Why is Beethoven's Ninth Symphony particularly remarkable?",
        "What does the Theory of Relativity help us understand?",
        "How are black holes formed?",
        "How did the Gutenberg Press impact society?",
        "How did Renaissance artists change the portrayal of the human form in their work?",
        "What aspects have contributed to the global popularity of tennis?"
    };

    public string[] HCLChoice1 = {
        "By incorporating digital tools and techniques.",
        "It uses multiple colors and paints.",
        "From simple pixelated graphics to immersive, lifelike experiences.",
        "It revolutionized communication, commerce, and information access.",
        "It was written for a small chamber orchestra.",
        "How chemical reactions occur.",
        "When massive stars collapse under their own gravity.",
        "It made books more accessible and affordable.",
        "They used abstract shapes and vivid colors.",
        "Exclusive club membership."
    };

    public string[] HCLChoice2 = {
        "By eliminating the need for any design principles.",
        "It involves mathematical principles and intricate patterns.",
        "From board games to card games.",
        "It limited access to information.",
        "Beethoven was completely deaf when he composed it.",
        "How objects move at high speeds and how gravity affects space-time.",
        "When planets collide.",
        "It increased the use of handwriting in books.",
        "They emphasized realism and perspective.",
        "Grand Slam events and influential players."
    };

    public string[] answers = {"A", "B", "A", "A", "B", "B", "A", "A", "B", "B"};

    void Start()
    {
    }

    void Update()
    {
        
    }
}