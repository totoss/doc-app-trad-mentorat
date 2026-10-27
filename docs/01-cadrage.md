# Appli d'apprentissage de l'arabe — Cadrage

26 octobre 2026

## Contexte

Webapp d'apprentissage de l'arabe pour francophones : back-end .NET, front Angular, PWA dès le squelette pour une intégration mobile ultérieure (pas d'application native pour l'instant).

L'arabe n'est pas une langue « comme les autres » à enseigner, et la plupart des apps généralistes le traitent mal. Ses spécificités conditionnent les features avant la stack : c'est par là que commence le cadrage.

## 1. Les spécificités du tunisien qui dictent les features

### Dialecte tunisien : décidé, avec deux prérequis

L'app enseigne le tunisien (code ISO 639-3 `aeb`) dans sa variante de Zarzis (Sud-Est), pas l'arabe standard. Le tunisien n'a pas d'orthographe officielle : les natifs l'écrivent en lettres arabes avec des conventions variables, et surtout en « arabizi » (lettres latines + chiffres : 7 = ح, 3 = ع).

Deux prérequis avant le premier mot saisi en base :

- Une **charte orthographique** en script arabe (écriture de ڨ, des voyelles réduites, des emprunts) : c'est la cible d'apprentissage.
- Un **système de translittération** unique, arabizi populaire recommandé : c'est ce que l'apprenant lira dans les messages de sa famille. Affiché comme béquille au début, retiré progressivement.

Le champ `variety` reste dans le modèle, à `aeb` par défaut, pour ne pas fermer la porte à d'autres parlers.

### L'écriture : un module à part entière

- 28 lettres × 4 formes (isolée / initiale / médiane / finale), lettres qui ne se lient pas à gauche, sens d'écriture. Le navigateur gère le *shaping*, mais l'apprenant doit l'apprendre explicitement.
- Trois lettres supplémentaires propres au dialecte : ڨ (g des emprunts), ڤ (v), پ (p), à prévoir dans l'alphabet, la police et le clavier.
- Voyelles courtes souvent réduites (*ktebt*, *khdemt*) et non écrites : pas de harakat dans les premières itérations, l'audio et l'arabizi sont les référents de prononciation. Le champ `arabic_vocalized` est prévu mais reste vide.
- Lettres solaires et lunaires (assimilation de l'article), hamza non écrite, tā' marbūṭa.
- Saisie : la plupart des francophones n'ont pas de clavier arabe. Clavier virtuel tunisien dès le MVP, et exercices de reconstruction (glisser les lettres) avant les exercices de frappe.

### La phonologie

ع ح ق خ غ ص ض ط ظ ء n'existent pas en français. Bonne nouvelle tunisienne : les interdentales ث ذ ظ sont conservées, donc l'alphabet s'enseigne presque entier. Dans le parler de Zarzis, ق se prononce /g/ dans presque tous les mots : on l'écrit ق et on l'enseigne comme « la lettre qui se lit g », sauf liste d'exceptions (voir la charte). Audio natif obligatoire, paires minimales (كلب / قلب), et plus tard « s'enregistrer et comparer ».

### La morphologie racine / schème

C'est la grande opportunité que les apps génériques ratent : montrer que كتب → كاتب / مكتب / مكتبة / كتاب partagent une racine est un accélérateur énorme. Cela impose de modéliser la **racine** et le **schème** dans le modèle de données dès le départ. La grammaire propre au tunisien (conjugaison n- / n-…-u, futur باش, négation ما…ش) s'enseigne en phase 2.

### Les emprunts, un atout pour les francophones

Le tunisien est truffé d'emprunts au français et à l'italien (تيليفون, بورطابل, كوجينة, بابور). Les taguer « mots transparents » donne au débutant des victoires rapides dès la première semaine.

## 2. Périmètre fonctionnel par phases

### MVP : ce qui fait qu'un utilisateur revient

Premier chantier : le back-office, parce que la production de contenu démarre avant tout le reste.

- **Back-office contenu** (rôle admin) : saisie des lettres, mots, phrases, leçons et exercices directement en base ; enregistrement audio depuis le navigateur ; statut brouillon / publié ; export JSON de sauvegarde.
- **Piste « Lire »** : alphabet tunisien (28 lettres + ڨ ڤ پ), groupé par forme (ب ت ث ن ي, puis ج ح خ, …) et ordonné par fréquence, audio, tracé animé (SVG), écriture guidée sur canvas (sans reconnaissance au début).
- **Piste « Parler »**, en parallèle dès le premier jour : phrases de survie en audio + arabizi, script arabe affiché mais non exigé. Les deux pistes fusionnent une fois l'alphabet acquis, pour éviter l'effet « je ne peux rien dire avant trois semaines ».
- Vocabulaire avec **répétition espacée** : algorithme FSRS plutôt que SM-2 (nettement meilleur, implémentations .NET existantes). Chaque carte : arabe, arabizi, français, audio, exemple, racine, tag « mot transparent ».
- 3 à 4 types d'exercices : QCM, appariement, phrase à trous, remise en ordre, avec le clavier virtuel tunisien. Correction tolérante (voir §4).
- Progression : objectif quotidien, série, file de révision du jour.
- Auth, profil, sauvegarde serveur.

### Phase 2

- Leçons structurées (unités A1 → A2) avec explications grammaticales en français : conjugaison n- / n-…-u, futur باش, négation ما…ش, interrogatifs (شنوة, وين, كيفاش).
- Dictée audio, saisie au clavier sans aide.
- PWA offline : révisions mises en cache dans IndexedDB, synchronisation au retour réseau.
- Dictionnaire par racine, filtre « mots transparents ».

### Phase 3

- Dialogues de lecture avec traduction au clic, contenus audio longs.
- « S'enregistrer et comparer » à la voix native. Pas d'évaluation automatique : aucun service (Azure Speech, Whisper) ne note le tunisien de façon fiable.
- Tuteur conversationnel LLM contraint à la charte orthographique, decks créés par l'utilisateur.

## 3. Architecture proposée

### Back (.NET)

ASP.NET Core Web API en monolithe modulaire, avec trois domaines bien séparés : `Content` (lettres, mots, phrases, leçons — écrit par les admins, lu par tous), `Learning` (état SRS, revues, progression — très écrit) et `Identity`. Clean architecture légère, sans sur-découpage au départ.

- **PostgreSQL** + EF Core : support Unicode solide, collations, full-text ; `jsonb` pratique pour les contenus d'exercices hétérogènes.
- Contenu en base, administré via une API admin : entités `Letter`, `Lexeme`, `Sentence`, `Lesson`, `Exercise` avec statut brouillon / publié et horodatage ; les apprenants ne voient que le publié. Seed JSON uniquement pour amorcer un environnement, export JSON pour la sauvegarde.
- Auth : ASP.NET Identity + JWT avec deux rôles (`learner`, `admin`) ; OpenIddict si un vrai serveur OAuth devient nécessaire.
- Audios et images en blob (Azure Blob, S3 ou MinIO), jamais en base ; normalisation du volume et encodage Opus/MP3 à l'upload.
- Job scheduler (Hangfire ou Quartz) pour les rappels et notifications push, pas pour le SRS lui-même (qui se calcule à la revue).

### Front (Angular)

Composants standalone + signals, `@angular/pwa` dès le squelette (gratuit à mettre tôt, pénible à ajouter tard).

- Module `admin` chargé à la demande et protégé par rôle : formulaires de saisie, enregistrement audio via `MediaRecorder`, prévisualisation des exercices tels que l'apprenant les verra.
- RTL : `dir="rtl"` au niveau du composant arabe, **propriétés CSS logiques** (`margin-inline-start` plutôt que `margin-left`) partout, sinon tout le CSS est à reprendre plus tard.
- Typographie : Noto Naskh Arabic ou Amiri (les deux couvrent ڨ ڤ پ), tailles généreuses — les harakat sont illisibles en dessous de ~24 px.
- Audio : `HTMLAudioElement` pour la lecture ; `MediaRecorder` dès le MVP côté admin, en phase 3 côté apprenant.
- Canvas pour l'écriture ; un service d'état local (IndexedDB via Dexie) pour la file de révision offline.

## 4. Les pièges techniques à régler dès le départ

- **Fixer la charte orthographique avant la première saisie** et la ranger dans le back-office : chaque incohérence saisie se retrouve dans les exercices.
- **Normalisation Unicode** : NFC partout. Pour comparer une réponse, enlever les harakat (plage U+064B–U+0652, U+0670), unifier les alifs (أ إ آ → ا), tolérer ى/ي et ة/ه en fin de mot, et ق/ڨ selon la charte. Sinon les exercices de saisie seront injustes.
- Ne jamais stocker les formes de présentation (U+FB50–U+FDFF) ; laisser le navigateur faire le *shaping*.
- Modéliser `Lexeme` avec : `arabic`, `arabic_vocalized` (vide dans les premières itérations), `latin` (arabizi), `french`, `pos`, `root_id`, `pattern`, `gender`, `plural` (les pluriels brisés sont irréguliers, il faut les stocker), `variety` (défaut `aeb`), `loan_origin` (fr / it / aucun), `audio_url`, `status`.
- Modéliser `Letter` avec un flag `extended` (ڨ ڤ پ), un groupe de forme et un ordre d'enseignement.
- Séparer `Card` (contenu) de `UserCard` (état SRS : `due`, `stability`, `difficulty`, `reps`) et garder un `ReviewLog` : c'est ce qui permettra plus tard de recalibrer l'algorithme sur les vraies données.

## 5. Ce qu'il faut trancher avant de coder

Trois décisions, par ordre d'impact :

- [x] **MSA ou dialecte ?** → **Tunisien, parler de Zarzis** (`aeb`). Décision pédagogique qui conditionne contenu, audio et outils vocaux.
- [x] **Qui produit le contenu ?** → **Auto-produit, saisi directement en base via le back-office** (pas de CSV intermédiaire). C'est le vrai goulot d'étranglement, bien plus que le code.
- [x] **Quel public ?** → **Débutant complet qui ne lit pas l'alphabet.**

Les trois points sont fixés ; l'étape suivante est la charte orthographique, puis une spec détaillée et un backlog priorisé pour le MVP, back-office en tête.

Voir aussi : `02-charte-orthographique.md`, `03-backlog-mvp.md`, `04-plan-back-end.md`.
