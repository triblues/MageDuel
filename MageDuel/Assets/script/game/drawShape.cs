using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class drawShape : MonoBehaviour
{

    //public Text mytext;
    enum direction
    {
        positive,//bot left to top right
        negative,//bot right to top left
        horizontal,
        vertical
    };

    [HideInInspector]
    public enum shape
    {
        horizontal_line,
        vertical_line,
        triangle,
        square,
        diamond,
        no_shape
    };
    //bool abc = false;
    public float delayTime = 1.0f;//the delay before the user draw another point
    public float distOffset = 1.0f;//the allow distance the last point to first point of the shape
    public float angleOffset = 5.0f;//the angle allow to have mistake for total angle of the shape
    public float denominatorOffset = 0.01f;

    [Tooltip("must be positive")]
    public int increaseLoopCount = 2;//the amount increase of the point list loop

    public float RangeOffset = 10.0f;//never use


    [Tooltip("minimum must be negative")]
    public float minOffset = -1.0f;

    [Tooltip("maximum must be positive")]
    public float maxOffset = 1.0f;

    List<Vector3> mypointList;//all the point the player draw
    List<Vector3> turningPointList;//contain all the turining point, eg, a square will have 4 turning point
    List<Vector3> allLineVector;//contain all the line vector,
    List<direction> allGradientList;//contain all the gradient
    List<float> allAngle;

    LineRenderer myline;
    bool isMousePressed;
    Vector3 mousePos;
    float mytime;
    direction current_direction;
    direction last_direction;
    shape myshape;

    void Start()
    {
        isMousePressed = false;
        myline = GetComponent<LineRenderer>();
        mypointList = new List<Vector3>();
        turningPointList = new List<Vector3>();
        allAngle = new List<float>();
        allLineVector = new List<Vector3>();
        allGradientList = new List<direction>();
        mytime = -1;
        myshape = shape.no_shape;

        //Vector2 a = new Vector2(1, 1);
        //Vector2 b = new Vector2(1, -3);
        //double angle = Mathf.Atan2(b.y - a.y, b.x - a.x) * 180 / Mathf.PI;



    }
    public shape getShape()
    {
        return myshape;
    }
    public void setShape()
    {
        myshape = shape.no_shape;
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            
            isMousePressed = true;

        }
        if (Input.GetMouseButtonUp(0))
        {
            isMousePressed = false;
            new_check_line();

            mypointList.RemoveRange(0, mypointList.Count);
            turningPointList.RemoveRange(0, turningPointList.Count);
            allLineVector.RemoveRange(0, allLineVector.Count);
            allAngle.RemoveRange(0, allAngle.Count);
            allGradientList.RemoveRange(0, allGradientList.Count);
            myline.SetVertexCount(0);

        }
       

        if (isMousePressed == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 1));

            float distance;
            xy.Raycast(ray, out distance);
            ray.GetPoint(distance).Set(ray.GetPoint(distance).x, ray.GetPoint(distance).y, 0);

            if (mypointList.Count > 0)
            {
                if (mypointList[mypointList.Count - 1] == ray.GetPoint(distance))//mousepos
                {
                    Debug.Log("in2");
                    return;//player never move their mouse
                }


            }

            if (mytime < Time.time)
            {
               
                mypointList.Add(ray.GetPoint(distance));
              
                myline.SetVertexCount(mypointList.Count);
                myline.SetPosition(mypointList.Count - 1, ray.GetPoint(distance));
                mytime = Time.time + delayTime;
            }
        }
    }


    float getGradient(Vector3 one, Vector3 two)
    {
        float result, numerator, denominator;
        numerator = two.y - one.y;
        denominator = two.x - one.x;


        if (denominator == 0)//cannot divide by zero
            return 999;//vertical gradient, a magic number, should be close to zero the denominator

        if (numerator == 0)
            return 0;

        result = numerator / denominator;

        if (Mathf.Abs(numerator) > Mathf.Abs(denominator) + denominatorOffset)
        {
            if (Mathf.Abs(result) >= Mathf.Abs(numerator))//denomantor close to zero
                return 999;//vertical line
        }

        return result;
    }
    void getDirection(float gradient)
    {
        if (gradient == 999)
        {
            current_direction = direction.vertical;
            //allGradientList.Add(current_direction);
            //Debug.Log("gradient: " + gradient.ToString());
            return;
            //return direction.vertical;
        }

        if (gradient >= minOffset && gradient <= maxOffset)//horizontal line, gradient will be close to zero
        {
            current_direction = direction.horizontal;


        }
        else if (gradient < minOffset)
        {
            current_direction = direction.negative;


        }
        else
        {
            current_direction = direction.positive;


        }
        //Debug.Log("gradient: " + gradient.ToString());
        //allGradientList.Add(current_direction);
    }


    void new_check_line()
    {

        float gradient;
        int num_of_turning_point = 0;

        if (mypointList.Count <= increaseLoopCount)
        {
            if (mypointList.Count <= 1)
            {
                Debug.Log("this is a point");

            }
            else
                determineShape(num_of_turning_point);
        }
        else
        {

            turningPointList.Add(mypointList[0]);//add the starting point
                                                 //abc = false;
            for (int i = 0; i < mypointList.Count; i += increaseLoopCount)
            {

                if (i >= mypointList.Count - increaseLoopCount)//reach the last loop
                {
                    //turningPointList.Add(mypointList[i]);//add the last point
                    break;
                }
                else if (i == 0)//the very first loop
                {
                    gradient = getGradient(mypointList[i], mypointList[i + increaseLoopCount]);
                    getDirection(gradient);
                    last_direction = current_direction;
                    allGradientList.Add(current_direction);

                    Debug.Log("last: " + last_direction.ToString() + " " + i.ToString() + " " + (i + increaseLoopCount).ToString());
                    Debug.DrawLine(mypointList[i], mypointList[i + increaseLoopCount], Color.blue);
                }
                else
                {
                    gradient = getGradient(mypointList[i], mypointList[i + increaseLoopCount]);
                    getDirection(gradient);
                    //Debug.Log("gradient: " + gradient.ToString());

                    if (last_direction != current_direction)//mean that is a turning point
                    {
                        last_direction = current_direction;
                        num_of_turning_point++;
                        turningPointList.Add(mypointList[i]);
                        allGradientList.Add(current_direction);

                    }
                    Debug.Log("last: " + last_direction.ToString() + " " + i.ToString() + " " + (i + increaseLoopCount).ToString());
                    Debug.DrawLine(mypointList[i], mypointList[i + increaseLoopCount], Color.blue);
                }
            }
            determineShape(num_of_turning_point);
        }


    }

    void determineShape(int num_of_turning_point)
    {
        if (num_of_turning_point >= 1)
        {
            if (Vector2.Distance(mypointList[0], mypointList[mypointList.Count - 1]) > distOffset)
            {
                //check if the first and last point is in the same area
              
                Debug.Log("no shape turning point: " + turningPointList.Count);
            }
            else
            {
                //mean first and last point is sort of connected
                Debug.Log("turning point: " + turningPointList.Count);

                if (turningPointList.Count == 3)
                {
                   
                    Debug.Log("triangle");
                }
                else if (turningPointList.Count >= 4)
                {
                    Debug.Log("gradient list: " + allGradientList.Count.ToString());

                    if (allGradientList[0] == allGradientList[2] && allGradientList[1] == allGradientList[3])
                    {
                        if (allGradientList[0] == direction.horizontal || allGradientList[0] == direction.vertical)
                        {
                            if (allGradientList[1] == direction.horizontal || allGradientList[1] == direction.vertical)
                            {

                                return;
                            }

                        }
                        else
                        {
                            if (allGradientList[1] == direction.positive || allGradientList[1] == direction.negative)
                            {

                                return;
                            }
                        }


                    }
                    else
                    {


                    }
                }

               

            }
        }
        else//num_of_turning_point is zero
        {
            //we know this is a line
            float gradient = getGradient(mypointList[0], mypointList[mypointList.Count - 1]);//first and last point
            getDirection(gradient);
            Debug.Log("gradient: " + gradient.ToString());
            //Debug.Log("current: " + current_direction.ToString());
            if (current_direction == direction.horizontal)
            {
                myshape = shape.horizontal_line;
                Debug.Log("horizontal line");
            }
            else if (current_direction == direction.vertical)
            {
                myshape = shape.vertical_line;
                Debug.Log("vertical line");
            }
            else
            {
                
                Debug.Log("this is a bug");
            }
        }
    }

}















