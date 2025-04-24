from flask import Flask, request, jsonify
import firebase_admin
from firebase import db_firestore
from google.cloud.firestore_v1.base_query import FieldFilter
from google.cloud.firestore_v1 import Increment
from google.cloud import firestore
import json
import math, random

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
    new_list = list(query)
    print(new_list)
    if not new_list:
        return jsonify({"error": "Username does not exist"}), 400   

    print(query)
    user = {}
    user_info = {}
    id = ""
    for doc in new_list:
        print(f"ID : {doc.id}, Data: {doc.to_dict()}")
        user = doc.to_dict()
        user_info["id"] = doc.id
        user_info["username"] = user['username']
        id = doc.id


    # dbpasswd = dbuser.get("password")


    # if (password == dbpasswd):
    #     print("authentication successful")
    # else:
    #     print("authentication failed")

    # TODO: do something if the authentication was a success....
    # user["id"] = id
    return jsonify(user_info), 200

# @app.route('/add_items', methods=['POST'])
# def add_item_to_user():
#     data = request.get_json()
#     item_name = data.get("itemId")
#     user_id = data.get("userId")
#
#
#     if not item_name:
#         return jsonify({'error': 'Missing item name'}), 400
#
#     dbitems = db_firestore.collection("items")
#
#     # Retrieve the first item in the query
#     # (assume that no other items will have the same name)
#     item_query = dbitems.where(filter=FieldFilter("name", "==", f"{item_name}")).limit(1).stream()
#     item_doc = next(item_query, None)
#
#     if not item_doc:
#         return jsonify({'error' : 'Item not found'}), 404
#
#     item_ref = dbitems.document(item_doc.id)
#     user_ref = db_firestore.collection('users').document(user_id)
#     user = user_ref.get()
#
#     if not user.exists:
#         return jsonify({'error': f'User with id {user_id} does not exist'}), 404
#
#     name = user.get("username");
#
#     user_ref.update({
#         'items': firestore.ArrayUnion([item_ref])
#     })
#
#     return jsonify({'message': f'Item "{item_name}" added to user {name}'})
#

@app.route("/modify_user/<user_id>", methods=['PUT'])
def modify_user(user_id):
    data = request.get_json()

    dbusers = db_firestore.collection("users")
    user = dbusers.document(user_id)

    user.set(data)
    return jsonify({"message", "User updated"}), 200

# TODO: make this retrieve the list of items from the items collection
# (not just the references from the user)
@app.route("/users/<user_id>/items", methods=['GET'])
def get_user_items(user_id):
    dbusers = db_firestore.collection("users")
    user_ref = dbusers.document(user_id)
    user_doc = user_ref.get()

    if not user_doc.exists:
        return jsonify({'error': 'User not found'}), 404

    user_data = user_doc.to_dict()
    item_refs = user_data.get('items', [])
    items_data = []

    for ref in item_refs:
        item_doc = ref.get()
        if item_doc.exists:
            item = item_doc.to_dict()
            item['id'] = item_doc.id
            items_data.append(item)

    return jsonify(items_data), 200

@app.route("/get_locations", methods=['GET'])
def get_locations():
    locations_ref = db_firestore.collection("locations")
    docs = locations_ref.stream()

    results = []
    for doc in docs:
        data = doc.to_dict()
        data['id'] = doc.id
        results.append(data)

    return jsonify(results), 200

@app.route("/push_locations", methods=['POST'])
def push_locations():
    batch = db_firestore.batch()
    data = request.get_json()
    locations_dict = data['locations']
    print(locations_dict)
    locations_ref = db_firestore.collection("locations")

    print("hello")
    for loc in locations_dict:
        # loc = locations_dict[0]
        new_ref = locations_ref.document()
        print("honk")
        batch.set(new_ref, {
                "name": loc['name'],
                "latitude": loc['latitude'],
                "longitude": loc['longitude'],
                "items": loc['items']
            })
        batch.commit()

    # return jsonify({"test"})
    return jsonify({"status": "success", "uploaded": len(locations_dict)});


def offset_coordinates(lat, lon, radius_m):
    # Random offset in meters within a circle
    angle = random.uniform(0, 2 * math.pi)
    r = radius_m * math.sqrt(random.uniform(0, 1))
    delta_lat = r / 111320  # ~meters per degree latitude
    delta_lon = r / (40075000 * math.cos(math.radians(lat)) / 360)
    return lat + delta_lat * math.cos(angle), lon + delta_lon * math.sin(angle)

@app.route("/generate_items", methods=["POST"])
def generate_items():
    locdb = db_firestore.collection("locations")
    locations = locdb.stream()
    itemsdb = db_firestore.collection("items")
    items = itemsdb.stream()
    item_names = []
    for item in items:
        item = item.to_dict()
        item_names.append(item["name"])

    for location in locations:
        for _ in range(3):  # Number of items per location
            loc_ref = locdb.document(location.id)
            loc_dict = location.to_dict()
            lat, lon = offset_coordinates(loc_dict["latitude"], loc_dict["longitude"], 10)
            item = {
                "name": random.choice(item_names),
                "lat": lat,
                "lon": lon,
            }
            loc_ref.update({
                    "items": firestore.ArrayUnion([item])
                })
            # db_firestore.collection("item_placements").add(item)

    return jsonify({"status": "success"})

@app.route("/add_xp", methods=["POST"])
def add_xp_to_user():
    data = request.get_json()
    print(data)
    user_id = data["user_id"]
    xp_add = data["xp"]
    xp_add = int(xp_add)
    user_doc = db_firestore.collection("users").document(user_id)
    user_ref = user_doc.get()

    if not user_ref.exists:
        return jsonify({"error": "User does not exist"}), 400

    user_doc.update({
        "xp": Increment(15)
        })

    return jsonify({"success": f"Successfully incremented user xp by {xp_add}"}), 200 

@app.route('/add_items', methods=['POST'])
def add_items():
    try:
        items = request.get_json()

        if not isinstance(items, list):
            return jsonify({"error": "Expected a list of items"}), 400

        batch = db_firestore.batch()

        for item in items:
            doc_ref = db_firestore.collection("items").document()  # Auto-generated ID
            batch.set(doc_ref, item)

        batch.commit()
        return jsonify({"status": "success", "message": f"{len(items)} items added"}), 200

    except Exception as e:
        return jsonify({"error": str(e)}), 500


