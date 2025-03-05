from flask import Flask, request, jsonify
import firebase_admin
from firebase import db_firestore
from google.cloud.firestore_v1.base_query import FieldFilter

app = Flask(__name__)

@app.route("/")
def index():
    return "Index Page"

@app.route("/hello")
def hello_world():
    return "Hello, World!"

# TODO: make this more expansive; let's receive JSON from the frontend
# this will give us more flexibility
# TODO: need to verify that the username chosen has not already been taken
@app.route("/add_user", methods=['POST'])
def add_user():
    data = request.get_json()
    print(data)

    # Extract fields; what fields?
    # I think a (non-personal) location of origin would actually be really cool; let's add that!
    # Note: let's just get the username
    username = data.get("username")
    fullname = data.get("fullname")
    location = data.get("location")
    # email = data.get("email")

    # make sure the username that the user has chosen has not already been taken
    user_ref = db_firestore.collection("users")
    print("do something interesting here")
    test_query = user_ref.where(filter=FieldFilter("username", "==", f"{username}")).stream()
    # print(test_query)
    query_results = list(test_query)
    # return an error if username already exists in database
    if query_results:
        return jsonify({"error": "Username already in use"}), 400   

    user = {
            "username" : username,
            "location" : location,
            "fullname" : fullname,
            "items" : [],
            "visited_locations" : []
            # "email" : email
            }

    # maybe store this more securely
    # probably doesn't matter right now...
    # Update: we're authenticating with Google so we really don't need this
    # Update to update: this looks to be a massive pain; let's leave it for now
    # password = data.get("password")

    entry_ref = user_ref.add(user)

    generated_id = entry_ref[1].id
    print(f"New entry added with ID: {generated_id}")

    return jsonify({"message" :f"Received name: {username}"}), 200

# @app.route("/get_user/<username>")
# def get_user(username):
#     user = db_firestore.collection("users").document(f"{username}").get()
#     return user.to_dict()

@app.route("/del_user", methods=['POST'])
def del_user():
    data = request.get_json()
    username = data.get("username")

    users_ref = db_firestore.collection("users")

    query = users_ref.where("username", "==", f"{username}").stream()

    old_value = {}
    for doc in query:
        old_value = doc.to_dict()
        doc.reference.delete()
        print(f"Deleted entry with ID: {doc.id}")

    return jsonify(old_value)


@app.route("/login", methods=['POST'])
def login():
    data = request.get_json()

    # for now, let's just login with a username
    # Assume we have already authenticated with Google
    # This will probably be the user's email
    # or the dedicated username they created for the game
    # username = data.get("username")
    username = data.get("username")
    # password = data.get("password")

    # TODO: need to check whether the user exists
    dbuser = db_firestore.collection("users")
    query = dbuser.where(filter=FieldFilter("username", "==", f"{username}")).stream()
    if not query:
        return jsonify({"error": "Username already in use"}), 400   

    print(query)
    user = {}
    for doc in query:
        print(f"ID : {doc.id}, Data: {doc.to_dict()}")
        user = doc.to_dict()

    # dbpasswd = dbuser.get("password")


    # if (password == dbpasswd):
    #     print("authentication successful")
    # else:
    #     print("authentication failed")

    # TODO: do something if the authentication was a success....
    return jsonify(user), 200

