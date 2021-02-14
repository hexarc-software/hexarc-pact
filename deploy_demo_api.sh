heroku container:login
heroku container:push web -a hexarc-demo-api
heroku container:release web -a hexarc-demo-api
