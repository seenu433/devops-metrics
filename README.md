# devops-metrics

Metric Exporter for HPA as detailed in the [post](https://github.com/seenu433/devops-diaries/blob/master/autoscale-agent-hpa.md)
### Build nginx image
docker build -t myregistry/nginx-reverseproxy:1.0 -f Nginx.Dockerfile .

docker push myregistry/nginx-reverseproxy:1.0

### Build metric-exporter  image

docker build -t myregistry/devops-metrics:1.0 .

docker push myregistry/devops-metrics:1.0

### Deployment Yaml

```yml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: devops-metrics
  labels:
    app: devops-metrics
spec:
  replicas: 1
  selector:
    matchLabels:
      app: devops-metrics
  template:
    metadata:
      labels:
        app: devops-metrics
    spec:
      containers:
        - name: devops-metrics
          image: myregistry/devops-metrics:1.0
          ports:
            - containerPort: 80
---
kind: Service
apiVersion: v1
metadata:
  name: devops-metrics
spec:
  selector:
    app: devops-metrics
  ports:
    - port: 80
      targetPort: 80
---
apiVersion: apps/v1
kind: Deployment
metadata:
  name: devops-proxy
  labels:
    app: devops-proxy
spec:
  replicas: 1
  selector:
    matchLabels:
      app: devops-proxy
  template:
    metadata:
      labels:
        app: devops-proxy
    spec:
      containers:
        - name: devops-proxy
          image: myregistry/nginx-reverseproxy:1.0
          ports:
            - containerPort: 443
---
kind: Service
apiVersion: v1
metadata:
  name: devops-proxy
spec:
  selector:
    app: devops-proxy
  ports:
    - port: 443
      targetPort: 443
```
