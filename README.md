# php-vegas-audio-renderer

Install using composer :
```
composer require pierreminiggio/vegas-audio-renderer
```

```php

use PierreMiniggio\VegasAudioRenderer\VegasAudioRenderer;

$renderer = new VegasAudioRenderer('C:\\Program Files\\VEGAS\\VEGAS Pro 17.0\\vegas170.exe');
$renderer->render('F:\\videos\\vlogs\\19 - TikTok Meme Poster\\Comment j\'ai automatisé mon TikTok (IL POSTE UN MEME PAR HEURE !).wmv', 'F:\\dev\\php-vegas-audio-renderer\\test.mp3');

```
