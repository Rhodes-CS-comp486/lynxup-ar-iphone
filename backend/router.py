from flask import Flask
import firebase_admin
import firebase.db_firestore

app = Flask(__name__)

@app.route("/")
def index():
    return "Index Page"

@app.route("/hello")
def hello_world():
    return "Hello, World!"

@app.route("/add_user/<username>")
def add_user(username):
    user = db_firestore.collection("users").document(f"{username}")
    return "Success"


