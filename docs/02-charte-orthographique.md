# Charte orthographique du tunisien

26 octobre 2026

Version 0 : des choix par défaut à valider. Cette charte fixe la graphie de référence de l'app, en script arabe (cible d'apprentissage) et en arabizi (béquille), pour le parler de Zarzis (Sud-Est tunisien). Toute entrée saisie en base la suit, et la normalisation des réponses (§4 du cadrage) en découle.

## 1. Principes

1. **Une graphie par mot.** Chaque lexème a une seule forme en base ; les variantes acceptées à la correction sont gérées par la normalisation, jamais par une seconde saisie.
2. **Le script arabe est la cible, l'arabizi la béquille.** L'arabizi est affiché dans les premières unités, puis masqué par défaut ; il n'est jamais demandé en réponse.
3. **Le parler de référence est celui de Zarzis** : il fixe la prononciation, l'arabizi et le vocabulaire. En script arabe, on écrit comme un natif soigneux : pas de hamza, voyelles courtes non écrites, interdentales conservées, ق étymologique même quand il se prononce g, lettres ڨ ڤ پ pour les g, v, p des emprunts.
4. **L'arabizi reflète la prononciation de Zarzis**, pas la graphie arabe : *gal* (قال), *ech-chams* (الشمس) ; chiffres 7 et 3, le 9 seulement pour les rares /q/ conservés ; emphatiques en majuscules.
5. **Pas de harakat dans les premières itérations** : la prononciation passe par l'audio et l'arabizi.
6. **En cas de doute, on tranche et on note la décision** dans la table de la section 6 ; la charte est affichée dans le back-office.

## 2. Consonnes

Les consonnes de l'alphabet standard (ا est traité avec les voyelles, section 3) plus trois lettres dialectales ; les interdentales ث ذ ظ sont écrites et prononcées. L'arabizi suit les habitudes francophones (*ch* pour ش, *ou* pour la voyelle longue) ; les emphatiques prennent une majuscule.

| Lettre | Son | Arabizi | Exemple | Arabizi | Français |
| --- | --- | --- | --- | --- | --- |
| ب | b | b | باب | bab | porte |
| ت | t | t | تونس | Tounes | Tunisie |
| ث | th (interdental sourd) | th | ثلاثة | thlatha | trois |
| ج | j | j | جديد | jdid | nouveau |
| ح | h pharyngal | 7 | حوت | 7out | poisson |
| خ | kh | kh | خبز | khobz | pain |
| د | d | d | دار | dar | maison |
| ذ | dh (interdental sonore) | dh | ذيب | dhib | loup |
| ر | r roulé | r | راجل | rajel | homme |
| ز | z | z | زيت | zit | huile |
| س | s | s | سوق | soug | marché |
| ش | ch | ch | شمس | chams | soleil |
| ص | s emphatique | S | صباح | Sba7 | matin |
| ض | d emphatique | D | ضيف | Dif | invité |
| ط | t emphatique | T | طاولة | Tawla | table |
| ظ | dh emphatique | DH | نظيف | nDHif | propre |
| ع | pharyngale sonore | 3 | عسلامة | 3aslema | bonjour |
| غ | gh (r grasseyé) | gh | غالي | ghali | cher |
| ف | f | f | فلوس | flous | argent |
| ق | g (q dans quelques mots classiques) | g (9 si /q/) | قهوة | gahwa | café |
| ڨ | g (emprunts) | g | ڨاتو | gatou | gâteau |
| ك | k | k | كرهبة | karhba | voiture |
| ل | l | l | ليل | lil | nuit |
| م | m | m | مدينة | mdina | ville |
| ن | n | n | نار | nar | feu |
| ه | h léger | h | هنا | hna | ici |
| و | w (consonne) | w | وقت | wagt | temps |
| ي | y (consonne) | y | يوم | youm | jour |
| ڤ | v (emprunts) | v | ڤيلا | villa | villa |
| پ | p (emprunts) | p | پيتزا | pitza | pizza |

ا و ي servent aussi de voyelles longues (section 3). La hamza ء n'est pas écrite (section 5).

## 3. Voyelles et harakat

Les voyelles longues s'écrivent avec ا و ي ; les voyelles courtes ne s'écrivent pas en script arabe, comme chez les natifs. L'arabizi, lui, note toutes les voyelles prononcées.

| Son | Script arabe | Arabizi | Exemple | Arabizi | Français |
| --- | --- | --- | --- | --- | --- |
| a long | ا | a | كتاب | ktab | livre |
| ou long | و | ou | حوت | 7out | poisson |
| i long | ي | i | زيت | zit | huile |
| a court | (rien) | a | كلب | kalb | chien |
| i court | (rien) | i | بنت | bint | fille |
| ou court | (rien) | o | خبز | khobz | pain |
| voyelle réduite | (rien) | e | كتبت | ktebt | j'ai écrit |
| e ouvert (emprunts, fin de mot) | ي ou ا | e | كيفاش | kifech | comment |

**Harakat** : aucun dans les premières itérations. Le champ `arabic_vocalized` reste vide et réservé à une version ultérieure ; la prononciation s'apprend par l'audio et l'arabizi.

## 4. Mots-outils et clitiques

Règle générale : en script arabe on suit l'usage de l'arabe standard pour ce qui se colle ou non ; en arabizi on écrit ce qu'on entend, avec un tiret aux frontières de clitiques.

| Élément | Script arabe | Arabizi | Exemple | Arabizi | Français |
| --- | --- | --- | --- | --- | --- |
| Article | ال toujours écrit, même assimilé | el- / assimilé | الشمس | ech-chams | le soleil |
| Négation | ما détaché + ش collé au verbe | ma … ch | ما مشيتش | ma mchitech | je ne suis pas allé |
| Futur | باش détaché | bech | باش نمشي | bech nemchi | j'irai |
| في (dans) | détaché | fi / fi + article | في الدار | fid-dar | à la maison |
| ب (avec, par) | collé | b- / bel- | بالكرهبة | bel-karhba | en voiture |
| ل (à, pour) | collé | l- / lel- / assimilé | للدار | led-dar | à la maison |
| و (et) | collé | w- | وانا | w-ena | et moi |
| Possessif | متاع détaché | mta3 | الكتاب متاعي | el-ktab mta3i | mon livre |
| Relatif | اللي | elli | الراجل اللي جا | er-rajel elli ja | l'homme qui est venu |
| Suffixes pronominaux | collés : ي ك و ها نا كم هم | -i -ek -ou -ha -na -kom -hom | كتابو | ktabou | son livre |
| Démonstratifs | هذا هاذي هاذوما | hedha hedhi hedhouma | هذا الراجل | hedha er-rajel | cet homme |
| Interrogatifs | شنوة وين كيفاش علاش وقتاش شكون قداش | chnouwa win kifech 3lech wagtech chkoun gaddech | وين الدار ؟ | win ed-dar ? | où est la maison ? |

## 5. Hamza, fins de mots, emprunts

**Hamza** : jamais écrite. À l'initiale, alif nu (انا *ena*, اسم *esm*) ; au milieu et à la fin, elle disparaît ou devient voyelle longue (راس *ras*, سما *sma*, ما *ma*). Exception : les mots repris tels quels de l'arabe standard où le natif la prononce (مسؤول *mas'oul*) ; dans ce cas l'arabizi note une apostrophe.

**Fins de mots** :

- Féminin en ة (مدينة *mdina*, كرهبة *karhba*) ; ه seulement quand on entend un h (وجه *wjeh*).
- Alif maqsura ى pour les verbes et mots-outils où l'usage l'a fixée (مشى *mcha*, على *3la*, حتى *7atta*) ; sinon ا.
- Le suffixe « son » s'écrit و (كتابو *ktabou*), pas ه.

**Emprunts** (français, italien) : transcription phonétique en script arabe, voyelles longues sur les syllabes nettes, emphatiques quand le natif les prononce ainsi.

| Français | Script arabe | Arabizi | Origine |
| --- | --- | --- | --- |
| téléphone | تيليفون | tilifoun | fr |
| portable | پورطابل | porTabl | fr |
| cuisine | كوجينة | koujina | it / fr |
| bateau | بابور | babour | it (*vapore*) |
| taxi | طاكسي | Taksi | fr |
| gâteau | ڨاتو | gatou | fr |
| villa | ڤيلا | villa | fr |

Ces mots portent `loan_origin` en base et le tag « mot transparent » dans l'app.

## 6. ق ou ڨ : mot par mot

Norme de référence : le parler de Zarzis, où ق se prononce /g/ dans presque tous les mots. En script arabe on garde ق : c'est ce que les natifs écrivent, et ce que l'apprenant lira sur les panneaux et dans les autres parlers ; l'arabizi note *g*. ڨ est réservé aux emprunts et aux mots dont le /g/ ne vient pas d'un ق. Les quelques mots classiques où /q/ est conservé sont à vérifier avec le locuteur ; la table grandit au fil de la saisie.

| Mot | Graphie retenue | Prononcé | Arabizi | Français | Note |
| --- | --- | --- | --- | --- | --- |
| قال | ق | g | gal | il a dit | ق étymologique |
| قهوة | ق | g | gahwa | café | ق étymologique |
| بقرة | ق | g | bagra | vache | ق étymologique, pas ڨ |
| قداش | ق | g | gaddech | combien | ق étymologique |
| قابس، قفصة | ق | g | Gabes, Gafsa | Gabès, Gafsa | graphie officielle |
| القرآن | ق | q | el-9or'an | le Coran | /q/ conservé, à vérifier |
| ڨاتو | ڨ | g | gatou | gâteau | emprunt |
| ڨراج | ڨ | g | garaj | garage | emprunt |

## 7. Choix validés

Décisions prises ; toute modification se fait directement dans les sections concernées.

- [x] Parler de référence : Zarzis. ق écrit, prononcé /g/ (arabizi *g*), ڨ réservé aux emprunts.
- [x] Arabizi francophone : *ch* pour ش, *ou* pour la voyelle longue, *e* pour la voyelle réduite.
- [x] Emphatiques en majuscules (S, D, T, DH) plutôt que non distinguées comme dans les SMS.
- [x] Pas de harakat dans les premières itérations ; champ `arabic_vocalized` réservé.
- [x] Négation : ما détaché, ش collé (ما مشيتش).
- [x] Pas de hamza, sauf mots repris de l'arabe standard et prononcés avec.
- [x] Démonstratifs écrits هذا هاذي هاذوما (et non هاذا).

## 8. Points ouverts, hors MVP

À trancher après le MVP, quand le contenu et un locuteur de référence seront en place. En attendant, le MVP applique les sections 1 à 7 telles quelles.

- [ ] في toujours détaché (في الدار), ب et ل collés (بالكرهبة, للدار) : à confirmer ou à aligner sur l'usage des natifs.
- [ ] پ écrit quand on entend un p (پيتزا), ou ب comme chez la plupart des natifs.
- [ ] Voix native des enregistrements : qui, et parle-t-elle le zarzisien ?
- [ ] Avec le locuteur : les mots où /q/ est conservé (registre religieux ou classique), à ajouter à la table de la section 6.
- [ ] Avec le locuteur : diphtongues conservées ou non (بيت *bayt* ou *bit*, يوم *yawm* ou *youm*) ; elles changent l'arabizi et les exemples des sections 2 et 3.
- [ ] Avec le locuteur : marques propres aux parlers du Sud (féminin à la 2ᵉ personne تكتبي, pluriel féminin des verbes), pour les tables de conjugaison de la phase 2.
- [ ] Avec le locuteur : vocabulaire courant qui diffère de Tunis (interrogatifs, salutations).
