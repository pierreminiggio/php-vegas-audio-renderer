<?php

namespace PierreMiniggio\VegasAudioRenderer;

use PierreMiniggio\VegasRenderer\FrFrVersion170Build387\Template\MP3\AudioMp3\MP3ModeleParDefaut;
use PierreMiniggio\VegasRenderer\VegasRenderer;

class VegasAudioRenderer
{

    private string $vegasPath;
    private VegasRenderer $renderer;

    public function __construct(string $vegaPath)
    {
        $this->vegasPath = $vegaPath;
        $this->renderer = new VegasRenderer($vegaPath);
    }

    public function render(string $videoPath, string $outputFilePath): void
    {
        $tmpPath = __DIR__ . DIRECTORY_SEPARATOR . '..' . DIRECTORY_SEPARATOR . 'tmp';
        $projectFilePath = null;

        if (! file_exists($tmpPath)) {
            mkdir($tmpPath);
        }

        $i = 0;
        while ($projectFilePath === null) {
            $i++;
            $fileName = $tmpPath . DIRECTORY_SEPARATOR . $i;
            $projectTxt = $fileName . '.txt';

            if (! file_exists($projectTxt)) {
                file_put_contents($projectTxt, $i);
                $projectFilePath = $fileName . '.veg';
            }
        }

        $argsStr = null;

        $args = [
            'projectFilePath' => $projectFilePath,
            'videoPath' => $videoPath,
            'outputFilePath' => $outputFilePath
        ];

        foreach ($args as $argName => $argValue) {
            $argsStr .= ($argsStr === null ? '?' : '&') . $argName . '=' . $argValue;
        }

        if (file_exists($outputFilePath)) {
            unlink($outputFilePath);
        }

        shell_exec(
            '"'
            . $this->vegasPath
            . '" -SCRIPT:"'
            . __DIR__
            . DIRECTORY_SEPARATOR
            . '..'
            . DIRECTORY_SEPARATOR
            . 'RenderProject'
            . DIRECTORY_SEPARATOR
            . 'RenderProject'
            . DIRECTORY_SEPARATOR
            . 'Class1.cs'
            . $argsStr
            . '"'
        );

        $this->renderer->render($projectFilePath, new MP3ModeleParDefaut(), $outputFilePath);

        if (file_exists($projectFilePath)) {
            unlink($projectFilePath);
        }

        if (file_exists($projectTxt)) {
            unlink($projectTxt);
        }
    }
}