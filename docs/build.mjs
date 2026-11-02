// Génère docs/index.html à partir des fichiers docs/*.md : node docs/build.mjs
// Le Markdown est embarqué tel quel et rendu dans le navigateur (marked + mermaid),
// donc la page s'ouvre directement en file:// sans serveur.
import { readFileSync, readdirSync, writeFileSync } from 'node:fs';
import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';

const dir = dirname(fileURLToPath(import.meta.url));
const docs = readdirSync(dir)
  .filter((f) => f.endsWith('.md'))
  .sort()
  .map((file) => {
    const md = readFileSync(join(dir, file), 'utf8');
    const title = (md.match(/^# (.+)$/m) || [, file])[1].trim();
    return { id: file.replace(/\.md$/, ''), title, md };
  });

// Empêche un "</script" présent dans le Markdown de fermer la balise d'embarquement.
const embed = (s) => s.replace(/<\/script/gi, '<\\/script');

const html = `<!doctype html>
<html lang="fr">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>Documentation du projet</title>
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Noto+Naskh+Arabic:wght@400;700&display=swap">
<style>
:root {
  --bg: #fbfaf7; --surface: #ffffff; --text: #1f2328; --muted: #5d6670;
  --border: #e3e0d8; --accent: #0f6b5c; --accent-soft: #e4f1ee; --code-bg: #f3f1ec;
  color-scheme: light;
}
@media (prefers-color-scheme: dark) {
  :root:not([data-theme="light"]) {
    --bg: #15181c; --surface: #1c2025; --text: #e6e8eb; --muted: #9aa3ad;
    --border: #2d333b; --accent: #5cc7b2; --accent-soft: #1e3a35; --code-bg: #22272e;
    color-scheme: dark;
  }
}
:root[data-theme="dark"] {
  --bg: #15181c; --surface: #1c2025; --text: #e6e8eb; --muted: #9aa3ad;
  --border: #2d333b; --accent: #5cc7b2; --accent-soft: #1e3a35; --code-bg: #22272e;
  color-scheme: dark;
}
* { box-sizing: border-box; }
body {
  margin: 0; background: var(--bg); color: var(--text);
  font: 16px/1.65 system-ui, -apple-system, "Segoe UI", Roboto, sans-serif;
}
a { color: var(--accent); }
.layout { display: grid; grid-template-columns: 260px minmax(0, 1fr) 220px; min-height: 100vh; }
nav.docs, aside.toc {
  position: sticky; top: 0; align-self: start; max-height: 100vh; overflow-y: auto;
  padding: 24px 16px; font-size: 14px;
}
nav.docs { border-inline-end: 1px solid var(--border); background: var(--surface); min-height: 100vh; }
nav.docs h1 { font-size: 15px; margin: 0 0 4px; }
nav.docs p { margin: 0 0 20px; color: var(--muted); font-size: 13px; }
nav.docs a, aside.toc a {
  display: block; padding: 6px 10px; border-radius: 6px; color: var(--text); text-decoration: none;
}
nav.docs a:hover, aside.toc a:hover { background: var(--accent-soft); }
nav.docs a[aria-current="page"] { background: var(--accent-soft); color: var(--accent); font-weight: 600; }
nav.docs .num { color: var(--muted); margin-inline-end: 6px; font-variant-numeric: tabular-nums; }
aside.toc h2 { font-size: 12px; text-transform: uppercase; letter-spacing: .06em; color: var(--muted); margin: 0 0 8px; }
aside.toc a { padding: 3px 10px; color: var(--muted); }
aside.toc a.h3 { padding-inline-start: 22px; font-size: 13px; }
main { padding: 32px 48px 80px; max-width: 980px; }
main h1 { font-size: 2rem; line-height: 1.2; margin: 0 0 4px; }
main h1 + p { color: var(--muted); margin-top: 0; }
main h2 { margin-top: 2.4em; padding-bottom: 6px; border-bottom: 1px solid var(--border); }
main h3 { margin-top: 1.8em; }
main h2, main h3 { scroll-margin-top: 16px; }
code { background: var(--code-bg); padding: .1em .35em; border-radius: 4px; font-size: .9em; }
pre { background: var(--code-bg); padding: 14px 16px; border-radius: 8px; overflow-x: auto; }
pre code { background: none; padding: 0; }
pre.mermaid { background: var(--surface); border: 1px solid var(--border); text-align: center; }
.table-wrap { overflow-x: auto; margin: 1em 0; border: 1px solid var(--border); border-radius: 8px; }
table { border-collapse: collapse; width: 100%; font-size: 14px; background: var(--surface); }
th, td { padding: 8px 12px; border-bottom: 1px solid var(--border); text-align: start; vertical-align: top; }
th { background: var(--code-bg); font-weight: 600; white-space: nowrap; }
tr:last-child td { border-bottom: 0; }
li:has(> input[type="checkbox"]) { list-style: none; margin-inline-start: -1.4em; }
input[type="checkbox"] { accent-color: var(--accent); margin-inline-end: 6px; }
.ar { font-family: "Noto Naskh Arabic", "Geeza Pro", serif; font-size: 1.3em; line-height: 1.4; unicode-bidi: isolate; }
.theme-btn {
  margin-top: 20px; width: 100%; padding: 6px 10px; border: 1px solid var(--border);
  border-radius: 6px; background: transparent; color: var(--text); font: inherit; cursor: pointer;
}
.menu-btn { display: none; }
@media (max-width: 1100px) { .layout { grid-template-columns: 240px minmax(0, 1fr); } aside.toc { display: none; } }
@media (max-width: 760px) {
  .layout { display: block; }
  nav.docs { position: static; min-height: 0; max-height: none; border-inline-end: 0; border-bottom: 1px solid var(--border); padding: 12px 16px; }
  nav.docs .links { display: none; }
  nav.docs.open .links { display: block; }
  .menu-btn { display: inline-block; float: inline-end; padding: 4px 10px; border: 1px solid var(--border); border-radius: 6px; background: transparent; color: var(--text); font: inherit; }
  nav.docs p { margin-bottom: 0; }
  main { padding: 20px 16px 60px; }
  main h1 { font-size: 1.6rem; }
}
</style>
</head>
<body>
<div class="layout">
  <nav class="docs" aria-label="Documents">
    <button class="menu-btn" aria-expanded="false">Menu</button>
    <h1>Appli d'apprentissage du tunisien</h1>
    <p>Documentation du projet</p>
    <div class="links">
${docs.map((d) => `      <a href="#${d.id}"><span class="num">${d.id.slice(0, 2)}</span>${d.title}</a>`).join('\n')}
      <button class="theme-btn" type="button">Basculer clair / sombre</button>
    </div>
  </nav>
  <main id="content"></main>
  <aside class="toc" aria-label="Sur cette page"><h2>Sur cette page</h2><div id="toc"></div></aside>
</div>
${docs.map((d) => `<script type="text/markdown" id="md-${d.id}">\n${embed(d.md)}\n</script>`).join('\n')}
<script src="https://cdn.jsdelivr.net/npm/marked@12.0.2/marked.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/mermaid@10.9.1/dist/mermaid.min.js"></script>
<script>
const ids = ${JSON.stringify(docs.map((d) => d.id))};
const main = document.getElementById('content');
const toc = document.getElementById('toc');
const nav = document.querySelector('nav.docs');
const AR = /[\\u0600-\\u06FF\\u0750-\\u077F\\uFB50-\\uFDFF\\uFE70-\\uFEFF]+(?:[\\s\\u060C\\u061F]+[\\u0600-\\u06FF\\u0750-\\u077F\\uFB50-\\uFDFF\\uFE70-\\uFEFF]+)*/g;

const slug = (s) => s.toLowerCase().normalize('NFD').replace(/[\\u0300-\\u036f]/g, '')
  .replace(/[^\\p{L}\\p{N}]+/gu, '-').replace(/^-|-$/g, '');

// Isole et agrandit chaque segment en arabe pour un affichage RTL correct au milieu du français.
function wrapArabic(root) {
  const walker = document.createTreeWalker(root, NodeFilter.SHOW_TEXT, {
    acceptNode: (n) => n.parentElement.closest('pre, script') ? NodeFilter.FILTER_REJECT : NodeFilter.FILTER_ACCEPT,
  });
  const nodes = [];
  while (walker.nextNode()) if (walker.currentNode.data.search(AR) !== -1) nodes.push(walker.currentNode);
  for (const node of nodes) {
    const frag = document.createDocumentFragment();
    let last = 0;
    node.data.replace(AR, (m, i) => {
      frag.append(node.data.slice(last, i));
      const span = document.createElement('span');
      span.className = 'ar'; span.lang = 'ar'; span.dir = 'rtl'; span.textContent = m;
      frag.append(span);
      last = i + m.length;
    });
    frag.append(node.data.slice(last));
    node.replaceWith(frag);
  }
}

function isDark() {
  const t = document.documentElement.dataset.theme;
  return t ? t === 'dark' : matchMedia('(prefers-color-scheme: dark)').matches;
}

async function render() {
  const [docId, anchor] = decodeURIComponent(location.hash.slice(1)).split('/');
  const id = ids.includes(docId) ? docId : ids[0];
  main.innerHTML = marked.parse(document.getElementById('md-' + id).textContent);

  main.querySelectorAll('table').forEach((t) => {
    const wrap = document.createElement('div');
    wrap.className = 'table-wrap';
    t.replaceWith(wrap); wrap.append(t);
  });
  main.querySelectorAll('code.language-mermaid').forEach((c) => {
    const pre = document.createElement('pre');
    pre.className = 'mermaid'; pre.textContent = c.textContent;
    c.parentElement.replaceWith(pre);
  });
  // Liens entre documents : "02-charte-orthographique.md" -> page correspondante.
  main.querySelectorAll('code').forEach((c) => {
    const m = c.textContent.match(/^(\\d\\d-[\\w-]+)\\.md$/);
    if (m && ids.includes(m[1])) {
      const a = document.createElement('a'); a.href = '#' + m[1];
      c.replaceWith(a); a.append(c);
    }
  });

  toc.innerHTML = '';
  const seen = {};
  main.querySelectorAll('h2, h3').forEach((h) => {
    // Deux titres identiques (ex. « Learning » aux §4 et §6) doivent garder des ancres distinctes.
    const base = slug(h.textContent);
    seen[base] = (seen[base] || 0) + 1;
    h.id = seen[base] > 1 ? base + '-' + seen[base] : base;
    const a = document.createElement('a');
    a.href = '#' + id + '/' + h.id; a.textContent = h.textContent; a.className = h.tagName.toLowerCase();
    toc.append(a);
  });
  wrapArabic(main);
  wrapArabic(toc);

  nav.querySelectorAll('.links a').forEach((a) => a.hash === '#' + id ? a.setAttribute('aria-current', 'page') : a.removeAttribute('aria-current'));
  nav.classList.remove('open');
  document.title = main.querySelector('h1')?.textContent + ' — Documentation';

  mermaid.initialize({ startOnLoad: false, theme: isDark() ? 'dark' : 'default' });
  await mermaid.run({ nodes: main.querySelectorAll('pre.mermaid') });

  const target = anchor && document.getElementById(anchor);
  target ? target.scrollIntoView() : window.scrollTo(0, 0);
}

document.querySelector('.menu-btn').addEventListener('click', (e) => {
  const open = nav.classList.toggle('open');
  e.currentTarget.setAttribute('aria-expanded', open);
});
document.querySelector('.theme-btn').addEventListener('click', () => {
  document.documentElement.dataset.theme = isDark() ? 'light' : 'dark';
  try { localStorage.setItem('theme', document.documentElement.dataset.theme); } catch {}
  render();
});
try { const t = localStorage.getItem('theme'); if (t) document.documentElement.dataset.theme = t; } catch {}
addEventListener('hashchange', render);
render();
</script>
</body>
</html>
`;

writeFileSync(join(dir, 'index.html'), html);
console.log(`docs/index.html généré (${docs.length} documents)`);
