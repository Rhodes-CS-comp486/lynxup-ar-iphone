from flask import Flask
import firebase_admin

app = Flask(__name__)

@app.route("/")
def hello_world():
    return "Index Page"

@app.route("/hello")
def hello():
    return "Hello, World!"


