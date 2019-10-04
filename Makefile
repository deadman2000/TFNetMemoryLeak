build:
	docker build -t tfmemleak .

sh: build
	docker run -it --rm tfmemleak bash

run: build
	docker run -it --rm tfmemleak

clean:
	docker image rm tfmemleak

.PHONY: build
