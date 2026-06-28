<?php
header('Content-Type: application/json; charset=utf-8');
header('Access-Control-Allow-Origin: *');

$jsonFile = __DIR__ . '/subscriptionServices.json';

$method = $_SERVER['REQUEST_METHOD'];

if ($method === 'GET') {
    if (!file_exists($jsonFile)) {
        http_response_code(404);
        echo json_encode(['success' => false, 'error' => 'Fișierul de servicii nu a fost găsit']);
        exit;
    }

    $data = json_decode(file_get_contents($jsonFile), true);

    if (isset($_GET['id'])) {
        // Returneaza detaliile unui serviciu specific, identificat prin id
        $id = $_GET['id'];
        foreach ($data as $plan => $services) {
            foreach ($services as $service) {
                if ($service['id'] === $id) {
                    echo json_encode([
                        'success' => true,
                        'service' => $service,
                        'plan'    => $plan
                    ]);
                    exit;
                }
            }
        }
        http_response_code(404);
        echo json_encode(['success' => false, 'error' => 'Serviciu negăsit']);
    } else {
        // Returneaza toate serviciile (lista plata, cu plan inclus)
        $all = [];
        foreach ($data as $plan => $services) {
            foreach ($services as $service) {
                $all[] = array_merge($service, ['plan' => $plan]);
            }
        }
        echo json_encode(['success' => true, 'services' => $all]);
    }

} elseif ($method === 'POST') {
    $body = json_decode(file_get_contents('php://input'), true);

    if (!$body || !isset($body['id'], $body['label'], $body['value'])) {
        http_response_code(400);
        echo json_encode(['success' => false, 'error' => 'Date incomplete sau invalide']);
        exit;
    }

    if (!file_exists($jsonFile)) {
        http_response_code(404);
        echo json_encode(['success' => false, 'error' => 'Fișierul de servicii nu a fost găsit']);
        exit;
    }

    $data    = json_decode(file_get_contents($jsonFile), true);
    $updated = false;

    foreach ($data as $plan => &$services) {
        foreach ($services as &$service) {
            if ($service['id'] === $body['id']) {
                $service['label'] = trim($body['label']);
                $service['value'] = trim($body['value']);
                $updated = true;
                break 2;
            }
        }
    }
    unset($services, $service);

    if ($updated) {
        $written = file_put_contents(
            $jsonFile,
            json_encode($data, JSON_PRETTY_PRINT | JSON_UNESCAPED_UNICODE)
        );
        if ($written === false) {
            http_response_code(500);
            echo json_encode(['success' => false, 'error' => 'Nu s-a putut salva fișierul']);
        } else {
            echo json_encode(['success' => true, 'message' => 'Serviciu actualizat cu succes']);
        }
    } else {
        http_response_code(404);
        echo json_encode(['success' => false, 'error' => 'Serviciu negăsit']);
    }
} else {
    http_response_code(405);
    echo json_encode(['success' => false, 'error' => 'Metodă nepermisă']);
}
