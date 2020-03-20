import { AfterAll, BeforeAll, setDefaultTimeout } from 'cucumber';

setDefaultTimeout(1000);

BeforeAll(() => {
  // StartLocalServer.onRandomPort(),
});

// AfterAll(() => actorCalled('Umbra').attemptsTo(StopLocalServer.ifRunning()));
