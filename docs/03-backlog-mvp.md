# Backlog MVP

26 octobre 2026

Backlog priorisé du MVP, découpé en épics et en stories, avec un ordre de livraison qui met le back-office en premier. Les tailles sont des ordres de grandeur pour une personne à plein temps (S : une journée, M : deux à trois jours, L : une semaine).

## 1. Objectif du MVP et critère de fin

Un débutant complet, sans clavier arabe, apprend l'alphabet tunisien et 300 mots en huit semaines, en révisant chaque jour grâce à la répétition espacée. Tout le contenu est produit dans le back-office, par toi, selon la charte orthographique.

Le MVP est fini quand :

- les 31 lettres et au moins 8 unités (300 mots, 150 phrases, tous avec audio) sont publiées depuis le back-office ;
- un nouvel inscrit peut parcourir les pistes « Lire » et « Parler », faire ses révisions du jour et voir sa progression ;
- 5 à 10 bêta-testeurs débutants ont tenu deux semaines d'usage quotidien et leurs retours bloquants sont corrigés.

## 2. Jalons de livraison

Six jalons en séquence ; chacun est utilisable avant de passer au suivant. Durées indicatives pour une personne à plein temps, soit 16 semaines au total.

1. **M0 Socle** (2 semaines) : squelettes back et front, auth, blob, normalisation arabe, modèle de contenu, seed des lettres. Stories T01–T06, C01–C03.
2. **M1 Back-office** (3 semaines) : saisie des lettres, mots, phrases et leçons, enregistrement audio dans le navigateur. La production de contenu démarre ici. Stories A01–A04.
3. **M2 Piste « Lire »** (3 semaines) : écran lettre avec tracé, parcours par groupes de forme, exercices lettre, lecture de mots courts, clavier virtuel. Stories L01–L03, L05, X06.
4. **M3 Piste « Parler » et SRS** (3 semaines) : unités de survie, écoute-répétition, FSRS, file de révision du jour. Stories P01–P02, S01–S04.
5. **M4 Exercices et progression** (3 semaines) : moteur d'exercice, QCM, appariement, phrase à trous, remise en ordre, objectif quotidien et série. Stories X01–X05, G01.
6. **M5 Bêta** (2 semaines) : tests, revue RTL et accessibilité, bêta fermée, correctifs. Stories Q01–Q03, puis les « Should » restants.

## 3. Backlog par épic

Une table par épic. Priorité MoSCoW : les « Must » font le MVP, les « Should » entrent si le jalon est dans les temps, les « Could » attendent la bêta.

### E0 — Socle technique

| ID | Story | Taille | Priorité | Statut |
| --- | --- | --- | --- | --- |
| T01 | Squelette back : solution .NET (API, Application, Domain, Infrastructure), EF Core + PostgreSQL, migrations, docker-compose local | M | Must | À faire |
| T02 | Squelette front : Angular standalone + signals, routing, `@angular/pwa`, thème RTL en propriétés logiques, police Noto Naskh Arabic | M | Must | À faire |
| T03 | Auth : ASP.NET Identity + JWT, rôles `learner` / `admin`, inscription, connexion, refresh token | M | Must | À faire |
| T04 | Stockage blob et endpoint d'upload audio : normalisation du volume, encodage Opus/MP3 | M | Must | À faire |
| T05 | CI : build, tests, lint, déploiement d'un environnement de test | S | Should | À faire |
| T06 | Bibliothèque de normalisation arabe (NFC, harakat, alifs, ى/ي, ة/ه, ق/ڨ) avec tests tirés de la charte | S | Must | À faire |

### E1 — Modèle de contenu

| ID | Story | Taille | Priorité | Statut |
| --- | --- | --- | --- | --- |
| C01 | Entités `Letter`, `Root`, `Lexeme`, `Sentence` : champs de la charte, `variety`, `loan_origin`, `audio_url`, statut brouillon / publié | M | Must | À faire |
| C02 | Entités `Unit`, `Lesson`, `Exercise` (contenu typé en `jsonb`), ordre d'enseignement | M | Must | À faire |
| C03 | Seed des 31 lettres : formes, groupe de forme, ordre d'enseignement, flag `extended` | S | Must | À faire |
| C04 | Export JSON du contenu publié (sauvegarde, réimport) | S | Should | À faire |

### E2 — Back-office

| ID | Story | Taille | Priorité | Statut |
| --- | --- | --- | --- | --- |
| A01 | Module `admin` Angular protégé par rôle, navigation, listes avec recherche et filtres (sans audio, brouillons) | M | Must | À faire |
| A02 | Formulaires lettre, racine, lexème, phrase : champs de la charte, arabizi, clavier virtuel intégré | L | Must | À faire |
| A03 | Enregistrement audio dans le navigateur (`MediaRecorder`), réécoute, upload, ré-enregistrement | M | Must | À faire |
| A04 | Éditeur de leçon : composer une unité à partir de lexèmes et phrases, exercices générés par défaut, prévisualisation côté apprenant | L | Must | À faire |
| A05 | Charte affichée dans le back-office et contrôle de saisie (caractère hors charte signalé) | S | Should | À faire |
| A06 | Workflow brouillon → publié et tableau « ce qui manque » (mots sans audio, phrases sans traduction) | S | Should | À faire |

### E3 — Piste « Lire »

| ID | Story | Taille | Priorité | Statut |
| --- | --- | --- | --- | --- |
| L01 | Écran lettre : quatre formes, audio, mot d'exemple, animation du tracé (SVG) ; données de tracé des 31 lettres | L | Must | À faire |
| L02 | Parcours par groupes de forme (ب ت ث ن ي, ج ح خ, …) dans l'ordre de fréquence, déblocage progressif | M | Must | À faire |
| L03 | Exercices lettre : son → lettre, lettre → son, repérer la lettre dans un mot, reconnaître la forme (initiale, médiane, finale) | M | Must | À faire |
| L04 | Écriture guidée sur canvas : tracer par-dessus le modèle, sans reconnaissance | M | Should | À faire |
| L05 | Lecture de mots courts : assembler des lettres glissées, lire puis écouter l'audio | M | Must | À faire |

### E4 — Piste « Parler »

| ID | Story | Taille | Priorité | Statut |
| --- | --- | --- | --- | --- |
| P01 | Écran phrase : audio, arabizi, script arabe, traduction ; unités de survie (salutations, se présenter, chiffres, famille, marché) | M | Must | À faire |
| P02 | Écoute-répétition : lecture normale et lente, replay, enchaînement des phrases d'une unité | S | Must | À faire |
| P03 | Bascule arabizi visible / masqué selon l'avancement de la piste « Lire » | S | Should | À faire |
| P04 | Mise en avant des mots transparents (tag `loan_origin`) | S | Could | À faire |

### E5 — Répétition espacée

| ID | Story | Taille | Priorité | Statut |
| --- | --- | --- | --- | --- |
| S01 | FSRS en .NET avec paramètres par défaut ; entités `UserCard`, `ReviewLog` | M | Must | À faire |
| S02 | API : file de révision du jour, enregistrement d'une revue (note 1 à 4), recalcul de l'échéance | M | Must | À faire |
| S03 | Écran de révision : carte recto / verso, audio, quatre boutons, compteur du jour | M | Must | À faire |
| S04 | Introduction des nouvelles cartes à la fin d'une unité, limite quotidienne réglable | S | Must | À faire |

### E6 — Exercices

| ID | Story | Taille | Priorité | Statut |
| --- | --- | --- | --- | --- |
| X01 | Moteur d'exercice générique : type, contenu `jsonb`, correction, feedback, enregistrement du résultat | M | Must | À faire |
| X02 | QCM : texte, audio → texte, texte → audio | S | Must | À faire |
| X03 | Appariement : arabe ↔ français, audio ↔ arabe | S | Must | À faire |
| X04 | Phrase à trous avec clavier virtuel et correction tolérante (normalisation T06) | M | Must | À faire |
| X05 | Remise en ordre : mots d'une phrase, lettres d'un mot | S | Must | À faire |
| X06 | Clavier virtuel tunisien : 28 lettres + ڨ ڤ پ, espace, retour, utilisable au doigt | M | Must | À faire |

### E7 — Progression et engagement

| ID | Story | Taille | Priorité | Statut |
| --- | --- | --- | --- | --- |
| G01 | Objectif quotidien, série, XP ; écran d'accueil « aujourd'hui » (révisions, prochaine leçon) | M | Must | À faire |
| G02 | Vue d'ensemble : progression par unité et par lettre | S | Should | À faire |
| G03 | Rappel quotidien par e-mail (job Hangfire ou Quartz) | S | Could | À faire |

### E8 — Qualité et lancement

| ID | Story | Taille | Priorité | Statut |
| --- | --- | --- | --- | --- |
| Q01 | Tests : unitaires (normalisation, FSRS), intégration API, un parcours de bout en bout | M | Must | À faire |
| Q02 | Revue RTL et accessibilité : navigation clavier, contrastes, tailles de police arabes | S | Should | À faire |
| Q03 | Bêta fermée : 5 à 10 débutants, collecte des retours, correctifs bloquants | M | Must | À faire |

## 4. Production de contenu, en parallèle

Ce n'est pas du développement, mais c'est le chemin critique : il démarre dès que M1 est livré et doit être fini avant M5. Chaque élément porte un enregistrement audio par la voix native.

| Élément | Volume MVP | Prêt pour | Note |
| --- | --- | --- | --- |
| Lettres | 31, avec tracé et mot d'exemple | M2 | seed C03 + audio |
| Mots | 300 à 400, avec racine et arabizi | M3 | ~50 par unité |
| Phrases | 150, avec traduction | M3 | ~20 par unité |
| Unités | 8 à 10 (survie, puis thèmes du quotidien) | M4 | composées dans A04 |
| Exercices | générés par unité, relus à la main | M4 | X01–X05 |

## 5. Explicitement hors MVP

Reporté à la phase 2 ou 3 du cadrage, pour tenir les 16 semaines :

- Mode hors ligne (cache IndexedDB, synchronisation) ; la PWA n'est qu'installable.
- Leçons de grammaire (conjugaison, futur, négation), dictée audio, dictionnaire par racine.
- Harakat et champ `arabic_vocalized`.
- « S'enregistrer et comparer », tuteur LLM, decks utilisateur, dialogues de lecture.
- Notifications push ; seul le rappel e-mail (G03) est envisagé, en « Could ».
- Les points ouverts de la charte (section 8 de `02-charte-orthographique.md`).
