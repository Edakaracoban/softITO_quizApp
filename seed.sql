INSERT INTO Categories ([Name], ImageUrl) VALUES
('English', '/img/category/english.png'),
('Math', '/img/category/math.png'),
('Grammar', '/img/category/grammar.png'),
('Science', '/img/category/science.png'),
('Geography', '/img/category/geography.png');


INSERT INTO TestTypes ([Name], ImageUrl) VALUES
('Vocabulary', '/img/quiz/evq.jpg'),
('Reading', '/img/quiz/erq.jpg'),
('Writing', '/img/quiz/ewq.png'),
('Algebra', '/img/quiz/Algebra.png'),
('Geometry', '/img/quiz/Geometry.png'),
('Arithmetic', '/img/quiz/Arithmetic.png'),
('Verbs',  '/img/quiz/Verbs.png'),
('Nouns',  '/img/quiz/Nouns.png'),
('Sentence Correction', '/img/quiz/SC.png'),
('Biology',  '/img/quiz/biologyy.png'),
('Chemistry',  '/img/quiz/Chemistry.png'),
('Physics',  '/img/quiz/Physics.png'),
('Continents', '/img/quiz/Continents.png'),
('Capitals', '/img/quiz/Capitals.png'),
('Maps', '/img/quiz/Capitals.png');

-- English: CategoryId = 1
INSERT INTO Quizzes (Title, CategoryId, TestTypeId) VALUES
('English Vocabulary Quiz', 1, 1),
('English Reading Quiz', 1, 2),
('English Writing Quiz', 1, 3);

-- Math: CategoryId = 2
INSERT INTO Quizzes (Title, CategoryId, TestTypeId) VALUES
('Algebra Basics', 2, 4),
('Geometry Practice', 2, 5),
('Arithmetic Quick Test', 2, 6);

-- Grammar: CategoryId = 3
INSERT INTO Quizzes (Title, CategoryId, TestTypeId) VALUES
('Verb Tenses Quiz', 3, 7),
('Noun Types Test', 3, 8),
('Sentence Correction Quiz', 3, 9);

-- Science: CategoryId = 4
INSERT INTO Quizzes (Title, CategoryId, TestTypeId) VALUES
('Intro to Biology', 4, 10),
('Basic Chemistry Quiz', 4, 11),
('Physics Principles Test', 4, 12);

-- Geography: CategoryId = 5
INSERT INTO Quizzes (Title, CategoryId, TestTypeId) VALUES
('World Continents Quiz', 5, 13),
('Country Capitals Quiz', 5, 14),
('Map Reading Practice', 5, 15);




INSERT INTO Questions (Text, OptionA, OptionB, CorrectOption, QuizId) VALUES
('Synonym of "quick"?', 'fast', 'slow', 'A', 1),
('Meaning of "huge"?', 'tiny', 'giant', 'B', 1),
('Opposite of "happy"?', 'sad', 'joyful', 'A', 1),
('Fill in: "She is a ___ person."', 'kind', 'cruel', 'A', 1),
('Which means the same as "silent"?', 'noisy', 'quiet', 'B', 1),
('What is another word for "big"?', 'large', 'little', 'A', 1),
('Which is not an adjective?', 'run', 'happy', 'A', 1),
('Choose correct: "I ___ to school."', 'go', 'goes', 'A', 1),
('Choose the noun:', 'table', 'fast', 'A', 1),
('Antonym of "strong"?', 'weak', 'powerful', 'A', 1);

INSERT INTO Questions (Text, OptionA, OptionB, CorrectOption, QuizId) VALUES
('Choose the correct sentence.', 'She go to the market.', 'She goes to the market.', 'B', 2),
('Which sentence is grammatically correct?', 'He don’t like tea.', 'He doesn’t like tea.', 'B', 2),
('Fill in the blank: "They ___ playing outside."', 'is', 'are', 'B', 2),
('Select the correct form:', 'I has a car.', 'I have a car.', 'B', 2),
('Choose the better closing line for a letter.', 'Bye.', 'Sincerely, John', 'B', 2),
('What is the correct punctuation?', 'What time is it', 'What time is it?', 'B', 2),
('Pick the correct word order.', 'Always she is late.', 'She is always late.', 'B', 2),
('Select the grammatically correct phrase.', 'a red big car', 'a big red car', 'B', 2),
('Choose the correct use of capital letters.', 'i like pizza.', 'I like pizza.', 'B', 2),
('Select the properly written sentence.', 'Lets go to school.', 'Let’s go to school.', 'B', 2);

INSERT INTO Questions (Text, OptionA, OptionB, CorrectOption, QuizId) VALUES
('Which sentence is best for a formal email opening?', 'Hey, what’s up?', 'Dear Sir or Madam,', 'B', 3),
('Choose the better thesis statement.', 'I like pizza.', 'Global warming affects us all.', 'B', 3),
('Which is more appropriate in an academic essay?', 'Gonna', 'Going to', 'B', 3),
('Pick the better sentence transition.', 'Next I want say...', 'Furthermore, I would like to say...', 'B', 3),
('Which sentence uses correct punctuation?', 'My hobbies are: reading writing and cooking.', 'My hobbies are: reading, writing, and cooking.', 'B', 3),
('Choose the sentence with proper paragraph structure.', 'First. Second. Third.', 'First, we begin with...', 'B', 3),
('What is a better way to end an essay?', 'That’s it.', 'In conclusion, the evidence shows...', 'B', 3),
('Which phrase is suitable for formal writing?', 'Cool stuff!', 'The findings are significant.', 'B', 3),
('Select the sentence with better clarity.', 'He did it.', 'The manager completed the report.', 'B', 3),
('Which sentence avoids repetition?', 'The cat was a cat.', 'The feline was calm.', 'B', 3);


INSERT INTO Questions (Text, OptionA, OptionB, CorrectOption, QuizId) VALUES
('Solve for x: 2x + 3 = 7', 'x = 2', 'x = 3', 'A', 4),
('Which is a linear equation?', 'y = 2x + 1', 'y = x² + 1', 'A', 4),
('What is the value of x in: x - 4 = 6?', 'x = 2', 'x = 10', 'B', 4),
('Simplify: 3(x + 2)', '3x + 2', '3x + 6', 'B', 4),
('What is the slope in y = 4x + 5?', '4', '5', 'A', 4),
('Factor: x² - 9', '(x - 3)(x + 3)', '(x - 9)(x + 1)', 'A', 4),
('Which is not an algebraic expression?', '3x + 2', '3 + 2 = 5', 'B', 4),
('Solve: 5x = 20', 'x = 4', 'x = 5', 'A', 4),
('Expand: (x + 1)(x + 2)', 'x² + 3x + 2', 'x² + 2x + 3', 'A', 4),
('Which represents an equation?', 'x + 5', 'x + 5 = 10', 'B', 4);


INSERT INTO Questions (Text, OptionA, OptionB, CorrectOption, QuizId) VALUES
('How many degrees in a triangle?', '180°', '90°', 'A', 5),
('A square has how many sides?', '4', '6', 'A', 5),
('What shape has no sides?', 'Circle', 'Triangle', 'A', 5),
('Which is a 3D shape?', 'Cube', 'Square', 'A', 5),
('How many right angles in a rectangle?', '2', '4', 'B', 5),
('Area of square with side 4?', '16', '8', 'A', 5),
('Perimeter of rectangle: 2x+3 and x sides?', '6x+6', '4x+6', 'B', 5),
('Name a polygon with 5 sides.', 'Pentagon', 'Hexagon', 'A', 5),
('Which is a type of angle?', 'Acute', 'Circle', 'A', 5),
('Which tool is used to measure angles?', 'Protractor', 'Compass', 'A', 5);


INSERT INTO Questions (Text, OptionA, OptionB, CorrectOption, QuizId) VALUES
('What is 8 + 5?', '13', '14', 'A', 6),
('What is 12 ÷ 3?', '4', '3', 'A', 6),
('Which is an even number?', '7', '8', 'B', 6),
('What is 9 x 6?', '54', '56', 'A', 6),
('What is 15 - 7?', '8', '7', 'A', 6),
('Which is a prime number?', '9', '11', 'B', 6),
('What is 100 ÷ 10?', '10', '5', 'A', 6),
('What is the sum of 3, 4, and 5?', '11', '12', 'B', 6),
('What is 5²?', '25', '10', 'A', 6),
('Which is correct: 6 x 7 = ?', '42', '36', 'A', 6);


INSERT INTO Questions (Text, OptionA, OptionB, CorrectOption, QuizId) VALUES
('Aşağıdaki cümlelerden hangisi şimdiki zamanı (present continuous) ifade eder?', 'Ben kitap okuyorum.', 'Ben kitabı okudum.', 'A', 7),
('“O dün sinemaya gitti.” cümlesinin İngilizce karşılığı hangisidir?', 'He went to the cinema yesterday.', 'He goes to the cinema yesterday.', 'A', 7),
('“Onlar her sabah koşarlar.” cümlesinde hangi zaman kullanılmıştır?', 'Simple Present', 'Present Continuous', 'A', 7),
('“Ben yarın okulda olacağım.” cümlesi hangi zamanla ifade edilir?', 'Future Tense', 'Past Tense', 'A', 7),
('“Geçen hafta yeni bir araba aldım.” cümlesinin zamanı nedir?', 'Past Simple', 'Present Perfect', 'A', 7),
('“Şu anda dışarıda yağıyor.” cümlesi İngilizce nasıl olur?', 'It is raining outside now.', 'It rains outside now.', 'A', 7),
('“Biz daha önce o filmi gördük.” cümlesi hangi zamandır?', 'Present Perfect', 'Past Simple', 'A', 7),
('“O her gün kahve içer.” cümlesinin İngilizcesi hangisidir?', 'He drinks coffee every day.', 'He is drinking coffee every day.', 'A', 7),
('“Onlar dün parkta oynuyorlardı.” cümlesi hangi zamanı anlatır?', 'Past Continuous', 'Simple Past', 'A', 7),
('“Sen şimdi ne yapıyorsun?” cümlesinin İngilizce karşılığı nedir?', 'What are you doing now?', 'What do you do now?', 'A', 7);


INSERT INTO Questions (Text, OptionA, OptionB, CorrectOption, QuizId) VALUES
('Aşağıdaki kelimelerden hangisi soyut isimdir?', 'sevgi', 'masa', 'A', 8),
('“Kitap” kelimesi hangi tür isimdir?', 'Somut isim', 'Soyut isim', 'A', 8),
('“Öğrenci” kelimesi hangi isim türüne girer?', 'Canlı varlık', 'Cansız varlık', 'A', 8),
('“Sevgi” kelimesi ne tür isimdir?', 'Soyut isim', 'Somut isim', 'A', 8),
('“Kalem” kelimesi aşağıdakilerden hangisidir?', 'Somut isim', 'Soyut isim', 'A', 8),
('“Adalet” kelimesi hangi tür isimdir?', 'Soyut isim', 'Somut isim', 'A', 8),
('“Elma” kelimesi ne tür isimdir?', 'Somut isim', 'Soyut isim', 'A', 8),
('“Hayvan” kelimesi hangi isim türüdür?', 'Genel isim', 'Özel isim', 'A', 8),
('“Türkiye” kelimesi hangi isim türüne girer?', 'Özel isim', 'Genel isim', 'A', 8),
('“Kütüphane” kelimesi ne tür isimdir?', 'Somut isim', 'Soyut isim', 'A', 8);


INSERT INTO Questions (Text, OptionA, OptionB, CorrectOption, QuizId) VALUES
('Aşağıdaki cümledeki hata nedir? "Ben dün sinemaya gidiyorum."', '"gidiyorum" yanlış zaman kullanımı.', '"dün" yanlış kelime.', 'A', 9),
('“O çok güzel konuşur.” cümlesinde hata var mı?', 'Hayır, doğru.', 'Evet, yanlış.', 'A', 9),
('“Sen kitap okuyacak mısın?” cümlesinin doğru hali hangisidir?', 'Sen kitap okuyacak mısın?', 'Sen kitap okuyacak mısın?', 'A', 9),
('“Biz yarın pikniğe gidiyoruz.” cümlesi doğrudur.', 'Doğru', 'Yanlış', 'A', 9),
('“Onlar dün okulda oynuyorlar.” cümlesinde hata nedir?', '"dün" ve şimdiki zaman uyumsuzluğu.', '"okulda" yanlış.', 'A', 9),
('“Ben yeni bir araba aldım.” cümlesi doğru mu?', 'Evet, doğru.', 'Hayır, yanlış.', 'A', 9),
('“Siz ne yapıyorsunuz?” cümlesinde hata var mı?', 'Hayır, doğru.', 'Evet, yanlış.', 'A', 9),
('“O kitapları okur.” cümlesi doğrudur.', 'Doğru', 'Yanlış', 'A', 9),
('“Biz her gün koşarız.” cümlesindeki hata nedir?', '"koşarız" doğru kullanılmış.', '"koşarız" yanlış.', 'A', 9),
('“Sen kitap okudun mu?” cümlesinin doğru hali nedir?', 'Sen kitap okudun mu?', 'Kitap okudun mu sen?', 'A', 9);


INSERT INTO Questions (Text, OptionA, OptionB, CorrectOption, QuizId) VALUES
('What is the basic unit of life?', 'Cell', 'Atom', 'A', 10),
('Which organelle is known as the powerhouse of the cell?', 'Mitochondria', 'Nucleus', 'A', 10),
('DNA stands for?', 'Deoxyribonucleic acid', 'Ribonucleic acid', 'A', 10),
('What do plants produce during photosynthesis?', 'Oxygen', 'Carbon dioxide', 'A', 10),
('Which system in the body fights diseases?', 'Immune system', 'Digestive system', 'A', 10),
('Where does cellular respiration take place?', 'Mitochondria', 'Chloroplast', 'A', 10),
('Humans have how many chromosomes?', '46', '23', 'A', 10),
('Which blood cells help in clotting?', 'Platelets', 'Red blood cells', 'A', 10),
('What is the function of roots in plants?', 'Absorb water', 'Produce food', 'A', 10),
('Which organ filters blood in the human body?', 'Kidney', 'Liver', 'A', 10);


INSERT INTO Questions (Text, OptionA, OptionB, CorrectOption, QuizId) VALUES
('What is the chemical symbol for water?', 'H2O', 'O2', 'A', 11),
('How many protons does an atom of carbon have?', '6', '12', 'A', 11),
('What is the pH value of pure water?', '7', '14', 'A', 11),
('Which gas is most abundant in the air?', 'Nitrogen', 'Oxygen', 'A', 11),
('What is NaCl commonly known as?', 'Salt', 'Sugar', 'A', 11),
('What kind of bond is formed when electrons are shared?', 'Covalent bond', 'Ionic bond', 'A', 11),
('Which element is a noble gas?', 'Neon', 'Hydrogen', 'A', 11),
('What is the atomic number?', 'Number of protons', 'Number of neutrons', 'A', 11),
('What happens in a chemical reaction?', 'New substances form', 'No change occurs', 'A', 11),
('Which state of matter has a definite shape?', 'Solid', 'Gas', 'A', 11);


INSERT INTO Questions (Text, OptionA, OptionB, CorrectOption, QuizId) VALUES
('What force pulls objects toward the Earth?', 'Gravity', 'Magnetism', 'A', 12),
('Speed is defined as?', 'Distance/time', 'Mass/volume', 'A', 12),
('What is the unit of force?', 'Newton', 'Joule', 'A', 12),
('Light travels fastest in?', 'Vacuum', 'Water', 'A', 12),
('Energy cannot be?', 'Created or destroyed', 'Transferred', 'A', 12),
('What is inertia?', 'Resistance to change in motion', 'Force of motion', 'A', 12),
('What causes tides on Earth?', 'Moon’s gravity', 'Sun’s heat', 'A', 12),
('Sound travels as?', 'Waves', 'Particles', 'A', 12),
('The formula F=ma is from which law?', 'Newton’s Second Law', 'Newton’s First Law', 'A', 12),
('Electric current is measured in?', 'Amperes', 'Volts', 'A', 12);


INSERT INTO Questions (Text, OptionA, OptionB, CorrectOption, QuizId) VALUES
('How many continents are there?', '7', '5', 'A', 13),
('Which continent is the largest?', 'Asia', 'Africa', 'A', 13),
('Antarctica is located at?', 'South Pole', 'North Pole', 'A', 13),
('Which continent is known as the "Dark Continent"?', 'Africa', 'Europe', 'A', 13),
('Which continent has the least population?', 'Antarctica', 'Australia', 'A', 13),
('Which continent is home to the Amazon rainforest?', 'South America', 'Africa', 'A', 13),
('Europe is separated from Asia by which mountain range?', 'Ural Mountains', 'Himalayas', 'A', 13),
('Australia is both a country and a?', 'Continent', 'Island', 'A', 13),
('Which continent is known for the Sahara Desert?', 'Africa', 'Asia', 'A', 13),
('The Great Barrier Reef is located in which continent?', 'Australia', 'South America', 'A', 13);


INSERT INTO Questions (Text, OptionA, OptionB, CorrectOption, QuizId) VALUES
('Capital of France?', 'Paris', 'London', 'A', 14),
('Capital of Japan?', 'Tokyo', 'Beijing', 'A', 14),
('Capital of Canada?', 'Ottawa', 'Toronto', 'A', 14),
('Capital of Australia?', 'Canberra', 'Sydney', 'A', 14),
('Capital of Brazil?', 'Brasília', 'Rio de Janeiro', 'A', 14),
('Capital of Egypt?', 'Cairo', 'Alexandria', 'A', 14),
('Capital of Germany?', 'Berlin', 'Munich', 'A', 14),
('Capital of India?', 'New Delhi', 'Mumbai', 'A', 14),
('Capital of Russia?', 'Moscow', 'Saint Petersburg', 'A', 14),
('Capital of South Africa?', 'Pretoria', 'Cape Town', 'A', 14);


INSERT INTO Questions (Text, OptionA, OptionB, CorrectOption, QuizId) VALUES
('What does a blue color on a map usually represent?', 'Water', 'Land', 'A', 15),
('Which symbol typically shows a city?', 'Dot', 'Triangle', 'A', 15),
('What does a compass rose indicate?', 'Direction', 'Distance', 'A', 15),
('What is the scale of a map used for?', 'Measuring distance', 'Showing climate', 'A', 15),
('What is a legend on a map?', 'Explanation of symbols', 'Map title', 'A', 15),
('Which line divides Earth into Northern and Southern Hemispheres?', 'Equator', 'Prime Meridian', 'A', 15),
('Which lines run from pole to pole?', 'Longitude', 'Latitude', 'A', 15),
('What type of map shows elevation?', 'Topographic', 'Political', 'A', 15),
('What does contour lines on a map show?', 'Elevation', 'Temperature', 'A', 15),
('Which direction does the prime meridian represent?', '0° Longitude', '0° Latitude', 'A', 15);


INSERT INTO UserQuizResults (UserId, QuizId, CorrectAnswers, TotalQuestions, TakenAt) VALUES
('gercek-kullanici-id-1', 1, 7, 10, DATEADD(day, -10, GETDATE())),
('gercek-kullanici-id-2', 3, 9, 10, DATEADD(day, -5, GETDATE())),
('gercek-kullanici-id-3', 5, 6, 10, DATEADD(day, -7, GETDATE()));
